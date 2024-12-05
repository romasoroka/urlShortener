using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using urlShortener.Controllers;
using urlShortener.Models;
using urlShortener.Data;
using System.Linq;
using Xunit;
using Moq;
using urlShortenerr.Controllers;

namespace urlShortenerr.Tests
{
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly DbContextOptions<UrlShortenerDbContext> _options;

        public HomeControllerTests()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();

            // using InMemory db for testing
            _options = new DbContextOptionsBuilder<UrlShortenerDbContext>()
                .UseInMemoryDatabase(databaseName: "UrlShortenerTestDb")
                .Options;
        }

        private UrlShortenerDbContext GetDbContext()
        {
            var context = new UrlShortenerDbContext(_options);
            context.UrlRecords.RemoveRange(context.UrlRecords); 
            context.SaveChanges();  
            return context;
        }

        [Fact]
        public void Create_View_Returns_View()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new HomeController(_mockLogger.Object, context);

            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result); 
        }

        [Fact]
        public void Delete_Url_Returns_View_For_Existing_Url()
        {
            // Arrange
            var context = GetDbContext();
            var url = new UrlRecord { OriginalUrl = "http://delete.com", ShortUrl = "del", CreatedBy = "admin" };
            context.UrlRecords.Add(url);
            context.SaveChanges();

            var controller = new HomeController(_mockLogger.Object, context);

            // Act
            var result = controller.Delete(url.Id) as ViewResult;

            // Assert
            Assert.NotNull(result);  
        }

        [Fact]
        public void DeleteConfirmed_Removes_Url()
        {
            // Arrange
            var context = GetDbContext();
            var url = new UrlRecord { OriginalUrl = "http://delete.com", ShortUrl = "del", CreatedBy = "admin" };
            context.UrlRecords.Add(url);
            context.SaveChanges();

            var controller = new HomeController(_mockLogger.Object, context);

            // Act
            var result = controller.DeleteConfirmed(url.Id) as RedirectToActionResult;
            context.SaveChanges(); 

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);  // check redirecting to index.cshtml
            Assert.Empty(context.UrlRecords);  // db need to be empty after removing
        }
    }
}
