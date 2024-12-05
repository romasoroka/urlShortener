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
        var currentUser = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Login == currentUserLogin);

        if (currentUser != null)
        {
            if (currentUser.Role == "admin") //if role is admin, view flag is true, the text can be changed
            {
                ViewBag.CanEdit = true;  
            }
        }
        return View("~/Views/AboutView/About.cshtml", _algorithmDescription);
    }



    [HttpPost]
    public IActionResult EditDescription(string algorithmDescription) 
    {
        if (string.IsNullOrEmpty(algorithmDescription))
        {
            ModelState.AddModelError("", "Description cannot be empty.");
            return View("~/Views/AboutView/About.cshtml", _algorithmDescription); 
        }
        _algorithmDescription = algorithmDescription;
        return RedirectToAction("About");
    }

}
