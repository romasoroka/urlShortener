using Moq;
using Microsoft.AspNetCore.Mvc;
using urlShortener.Controllers;
using urlShortener.Data;
using urlShortener.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace urlShortener.Tests
{
    //public class HomeControllerTests
    //{
    //    [Fact]
    //    public void Index_ReturnsViewResult_WithListOfUrls()
    //    {
    //        // Arrange
    //        var mockDbSet = new Mock<DbSet<UrlRecord>>();
    //        var mockContext = new Mock<UrlShortenerDbContext>();
    //        var mockLogger = new Mock<ILogger<HomeController>>();

    //        var urlRecords = new List<UrlRecord>
    //        {
    //            new UrlRecord { Id = 1, OriginalUrl = "http://example1.com", ShortUrl = "short1", CreatedBy = "user1" },
    //            new UrlRecord { Id = 2, OriginalUrl = "http://example2.com", ShortUrl = "short2", CreatedBy = "user2" }
    //        }.AsQueryable();

    //        mockDbSet.As<IQueryable<UrlRecord>>().Setup(m => m.Provider).Returns(urlRecords.Provider);
    //        mockDbSet.As<IQueryable<UrlRecord>>().Setup(m => m.Expression).Returns(urlRecords.Expression);
    //        mockDbSet.As<IQueryable<UrlRecord>>().Setup(m => m.ElementType).Returns(urlRecords.ElementType);
    //        mockDbSet.As<IQueryable<UrlRecord>>().Setup(m => m.GetEnumerator()).Returns(urlRecords.GetEnumerator());

    //        mockContext.Setup(c => c.UrlRecords).Returns(mockDbSet.Object);

    //        var controller = new HomeController(mockLogger.Object, mockContext.Object);

    //        // Act
    //        var result = controller.Index();

    //        // Assert
    //        var viewResult = Assert.IsType<ViewResult>(result);
    //        var model = Assert.IsAssignableFrom<List<UrlViewModel>>(viewResult.Model);
    //        Assert.Equal(2, model.Count);
    //    }
    //}
}
