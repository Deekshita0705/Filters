using Filters.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Filters.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [CustomLoggingActionFilter]
        public IActionResult Index()
        {
            return View();
        }

        [CustomExceptionFilter]
        public IActionResult Privacy()
        {
            int[] numbers = { 42 };
            int value = numbers[1];
            return View();
        }
        [CustomResultFilter]
        public IActionResult Result()
        {
            ViewBag.Message = "Welcome to the Result Page!";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}