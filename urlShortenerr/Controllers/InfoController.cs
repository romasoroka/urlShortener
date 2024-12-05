using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;

namespace urlShortenerr.Controllers
{
    public class InfoController : Controller
    {
        private readonly UrlShortenerDbContext _dbContext;

        public InfoController(UrlShortenerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Info/InfoPage/5
        public IActionResult InfoPage(int id)
        {
            var url = _dbContext.UrlRecords.FirstOrDefault(u => u.Id == id);
            if (url == null)
            {
                return NotFound(); // Return 404 if the URL is not found
            }

            // Map the URL entity to ViewModel
            var viewModel = new UrlViewModel
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortUrl = url.ShortUrl,
                CreatedBy = url.CreatedBy,
            };

            return View(viewModel); // Return the InfoPage view with the URL data
        }
    }
}
