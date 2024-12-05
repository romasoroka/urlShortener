using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using urlShortener.Data;
using urlShortenerr.Controllers;
using urlShortenerr.Models;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UrlShortenerDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, UrlShortenerDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var currentUserLogin = Request.Cookies["UserLogin"];
        int? currentUserId = null;
        bool isAdmin = false;

        if (currentUserLogin != null)
        {
            ViewBag.IsAuthenticated = true; 
            ViewBag.UserName = currentUserLogin;  

            var currentUser = _dbContext.ApplicationUsers
                                        .FirstOrDefault(u => u.Login == currentUserLogin);

            if (currentUser != null)
            {
                isAdmin = currentUser.Login == "admin";  
            }
        }
        else
        {
            ViewBag.IsAuthenticated = false;  
        }

        var urls = _dbContext.UrlRecords
            .Select(url => new UrlViewModel
            {
                Id = url.Id,
                OriginalUrl = url.OriginalUrl,
                ShortUrl = url.ShortUrl,
                CreatedBy = url.CreatedBy,
                CanEdit = isAdmin || currentUserLogin == url.CreatedBy
            })
            .ToList(); 

        return View(urls);  
    }

    public IActionResult Create()
    {
        return View();
    }

    // POST: UrlShortener/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(UrlRecord model)
    {
        var currentUserLogin = Request.Cookies["UserLogin"];
        model.CreatedBy = Request.Cookies["UserLogin"];
        ViewBag.ErrorMessage = "model" + model.CreatedBy;

        if (model.OriginalUrl != null && model.ShortUrl != null && currentUserLogin != null)
        {
            if(!_dbContext.UrlRecords.Any(u => u.OriginalUrl == model.OriginalUrl || u.ShortUrl == model.ShortUrl)){
                _dbContext.Add(model);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            else ViewBag.ErrorMessage = "The same url is already exist in DB";
        }
        else
        {
            ViewBag.ErrorMessage = "Invalid input. Please correct the errors and try again.";
        }
        return View(model);
    }
    public IActionResult Edit(int id)
    {
        // Retrieve the URL by Id
        var url = _dbContext.UrlRecords.FirstOrDefault(u => u.Id == id);
        if (url == null)
        {
            return NotFound(); // If the URL is not found, return a 404 page
        }

        // Map the URL entity to ViewModel
        var viewModel = new UrlViewModel
        {
            Id = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortUrl = url.ShortUrl
        };

        return View(viewModel); // Return the edit view with the URL data
    }

    // POST: /Home/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(UrlViewModel model)
    {
        if (model.OriginalUrl != null && model.ShortUrl != null)
        {           
            var url = _dbContext.UrlRecords.FirstOrDefault(u => u.Id == model.Id);
            if (url == null)
            {
                return NotFound(); 
            }

            if (url.OriginalUrl != model.OriginalUrl || url.ShortUrl != model.ShortUrl)
            {
                url.OriginalUrl = model.OriginalUrl;
                url.ShortUrl = model.ShortUrl;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

           ViewBag.ErrorMessage = "No changes were made to the URL.";
        }
        else
        {
            ViewBag.ErrorMessage = "Invalid input. Please correct the errors and try again.";
        }
        return View(model);
    }

    // GET: /Home/Delete/5
    public IActionResult Delete(int id)
    {
        var url = _dbContext.UrlRecords.FirstOrDefault(u => u.Id == id);

        if (url == null)
        {
            return NotFound(); // ���� URL �� ��������
        }

        return View(url); // ��������� ������ ��� ������������ ���������
    }
    
    // POST: /Home/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var url = _dbContext.UrlRecords.FirstOrDefault(u => u.Id == id);

        if (url == null)
        {
            return NotFound(); // ���� URL �� ��������
        }

        // ��������� URL � ���� �����
        _dbContext.UrlRecords.Remove(url);
        _dbContext.SaveChanges();

        return RedirectToAction("Index"); // ����������� �� ������� �������
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
