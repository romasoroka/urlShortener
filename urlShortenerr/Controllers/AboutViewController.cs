using Microsoft.AspNetCore.Mvc;
using System.Linq;
using urlShortener.Data;

public class AboutViewController : Controller
{
    private readonly UrlShortenerDbContext _dbContext;
    private static string _algorithmDescription = "The URL Shortener algorithm takes a long URL, processes it through a hashing mechanism, and then generates a unique short code associated with the original URL. This short code is stored in our database and can be used to retrieve the original URL by redirecting the user to the long URL when they visit the shortened link."; // Опис алгоритму

    public AboutViewController(UrlShortenerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult About()
    {
        var currentUserLogin = Request.Cookies["UserLogin"];

        // Перевірка на наявність користувача в базі
        var currentUser = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Login == currentUserLogin);

        if (currentUser != null)
        {
            if (currentUser.Role == "admin")
            {
                ViewBag.CanEdit = true;  
            }
        }

        // Повертаємо опис алгоритму для відображення
        return View("~/Views/AboutView/About.cshtml", _algorithmDescription);
    }



    // Дія для редагування опису алгоритму
    [HttpPost]
    public IActionResult EditDescription(string algorithmDescription)
    {
        // Перевірка на порожній опис
        if (string.IsNullOrEmpty(algorithmDescription))
        {
            ModelState.AddModelError("", "Description cannot be empty.");
            return View("~/Views/AboutView/About.cshtml", _algorithmDescription); // Повертаємо користувача на сторінку About з помилкою
        }

        // Оновлення опису
        _algorithmDescription = algorithmDescription;


        // Повертаємо на сторінку "About" з оновленим описом
        return RedirectToAction("About");
    }

}
