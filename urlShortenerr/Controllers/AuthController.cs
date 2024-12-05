using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;
using System.Linq;
using System.Threading.Tasks;

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
            // Перевірка наявності користувача в базі даних за логіном і паролем
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Login == username && u.Password == password);

            if (user != null)
            {
                Response.Cookies.Append("UserLogin", user.Login, new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true 
                });

                // Редірект на головну сторінку після успішного входу
                return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Якщо користувач не знайдений, вивести помилку
                TempData["ErrorMessage"] = "Invalid login attempt.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserLogin");
            return RedirectToAction("Index", "Home");
        }
    }
}
