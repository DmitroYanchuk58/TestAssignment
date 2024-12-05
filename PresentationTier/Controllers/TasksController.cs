using Microsoft.AspNetCore.Mvc;

namespace PresentationTier.Controllers
{
    public class TasksController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
