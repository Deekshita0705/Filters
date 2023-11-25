using Filters.Models;
using Microsoft.AspNetCore.Mvc;


namespace Filters.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AuthenticationFilter]
        public ActionResult Login(string email, string password)
        {
            // The actual logic is now handled by the AuthenticationFilterAttribute
            return View();
        }

    }
}
