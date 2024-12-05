using Microsoft.AspNetCore.Mvc;
using urlShortener.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            //check if user is not already registrated
            if (_context.ApplicationUsers.Any(u => u.Login == model.Login))
            {
                ViewBag.ErrorMessage = "User's already exist!";
                return View(model);
            }
            model.Role = "user"; // in this version, you can only registrate common users, not admins

            _context.ApplicationUsers.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Login", "Auth");

        }
    }
}
