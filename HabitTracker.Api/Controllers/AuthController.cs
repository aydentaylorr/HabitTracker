using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Api.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
