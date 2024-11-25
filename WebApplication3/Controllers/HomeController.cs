using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Retrieve "UserPreference" from the cookie
            if (Request.Cookies.TryGetValue("UserPreference", out string userPreference))
            {
                ViewBag.UserPreference = userPreference;
            }
            else
            {
                ViewBag.UserPreference = "Default"; // Default value if no cookie is set
            }

            // Retrieve "UserName" from the session if available
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(); // Return the "Index" view
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetUserName(string userName)
        {
            // Ensure "HttpContext.Session.SetString" is properly called
            HttpContext.Session.SetString("UserName", userName);

            // Redirect to the "Index" action
            return RedirectToAction("Index");
        }
        public IActionResult SetCookie(string preference)
        {
            // Set the cookie with options
            Response.Cookies.Append(
                "UserPreference",
                preference,
                new CookieOptions
                {
                    Expires = DateTimeOffset.Now.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true
                }
            );

            // Redirect or return a response
            return RedirectToAction("Index");
        }
    }
}
