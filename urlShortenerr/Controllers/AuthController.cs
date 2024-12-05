using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using urlShortener.Models;

namespace urlShortenerr.Controllers
{
    public class AuthController : Controller
    {
        private readonly UrlShortenerDbContext _context;

        public AuthController(UrlShortenerDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // check if user is registrated
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Login == username && u.Password == password);

            if (user != null)
            {
                Response.Cookies.Append("UserLogin", user.Login, new CookieOptions //making cookies for user login
                {
                    Expires = DateTime.Now.AddDays(1),
                    HttpOnly = true 
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "User is not registrated!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserLogin");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(ApplicationUser model)
        {
            // Check if the user is already registered
            if (_context.ApplicationUsers.Any(u => u.Login == model.Login))
            {
                ViewBag.ErrorMessage = "User already exists!";
                return View(model);
            }

            // Assign the selected role from the form
            if (model.Role == null) model.Role = "user"; // Default to "user" if no role is selected

            _context.ApplicationUsers.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Login", "Auth");
        }

    }
}
