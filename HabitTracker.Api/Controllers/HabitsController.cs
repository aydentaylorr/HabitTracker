using Microsoft.AspNetCore.Mvc;

namespace HabitTracker.Api.Controllers
{
    public class HabitsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
