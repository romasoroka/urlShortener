using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;
using urlShortener.Models;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using urlShortenerr.Controllers;

namespace urlShortenerr.Tests
{
    public class AuthControllerTests
    {
        private readonly UrlShortenerDbContext _context;

        public AuthControllerTests()
        {
            var options = new DbContextOptionsBuilder<UrlShortenerDbContext>()
                .UseInMemoryDatabase("UrlShortenerTestDb")
                .Options;

            _context = new UrlShortenerDbContext(options);
        }

        // Test autorization with correct login and password
        [Fact]
        public async Task Login_Post_Returns_Redirect_When_Valid_User()
        {
            var controller = new AuthController(_context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()  
            };

            var user = new ApplicationUser
            {
                Login = "testuser",
                Password = "password123",
                Role = "user"  
            };

            _context.ApplicationUsers.Add(user);
            await _context.SaveChangesAsync();  

            var result = await controller.Login("testuser", "password123") as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);  //check if redirect to Index.cshtml

            var cookieExists = controller.HttpContext.Response.Headers.ContainsKey("Set-Cookie");
            Assert.True(cookieExists);  
        }


        // Test log out function, clearing cookies and redirecting to index page
        [Fact]
        public void Logout_Removes_UserLogin_Cookie_And_Redirects_To_Home()
        {
            var controller = new AuthController(_context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()  
            };

            controller.HttpContext.Response.Cookies.Append("UserLogin", "testuser", new CookieOptions { Expires = DateTime.Now.AddDays(1) });

            var result = controller.Logout() as RedirectToActionResult;
            Assert.Equal("Index", result.ActionName);  // have to be redirected to Home/Index

            var cookieExists = controller.HttpContext.Request.Cookies.ContainsKey("UserLogin");
            Assert.False(cookieExists);  //cookies have to be cleared
        }

        // Test registrating new user
        [Fact]
        public void Register_Post_Adds_User_When_Valid_Data()
        {
            // Arrange
            var controller = new AuthController(_context);
            var newUser = new ApplicationUser
            {
                Login = "newuser",
                Password = "newpassword123",
                Role = "user"
            };

            // Act
            var result = controller.Register(newUser) as RedirectToActionResult;
            _context.SaveChanges();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);  // Redirect to Login after successful registration
            Assert.True(_context.ApplicationUsers.Any(u => u.Login == "newuser"));  // User should be saved in DB
        }

        // Test to registrate an existing user
        [Fact]
        public void Register_Post_Returns_View_When_User_Already_Exists()
        {
            // Arrange
            var controller = new AuthController(_context);
            var existingUser = new ApplicationUser
            {
                Login = "existinguser",
                Password = "password123",
                Role = "user"
            };
            _context.ApplicationUsers.Add(existingUser);
            _context.SaveChanges();

            var newUser = new ApplicationUser
            {
                Login = "existinguser",  // Same login as existing user
                Password = "newpassword123",
                Role = "user"
            };

            // Act
            var result = controller.Register(newUser) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("User's already exist!", result.ViewData["ErrorMessage"]);
        }
    }
}
