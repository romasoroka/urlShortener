using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;
using System.Linq;

namespace urlShortener.Controllers
{
    public class RedirectController : Controller
    {
        private readonly UrlShortenerDbContext _context;

        public RedirectController(UrlShortenerDbContext context)
        {
            _context = context;
        }

        [Route("{shortUrl}")]
        public IActionResult RedirectToOriginalUrl(string shortUrl)
        {
            var urlRecord = _context.UrlRecords.FirstOrDefault(u => u.ShortUrl == shortUrl);

            if (urlRecord == null)
            {
                return NotFound("The short URL does not exist.");
            }

            return Redirect(urlRecord.OriginalUrl);
        }
    }
}
