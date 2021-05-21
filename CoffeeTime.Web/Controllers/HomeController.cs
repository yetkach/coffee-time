using Microsoft.AspNetCore.Mvc;

namespace CoffeeTime.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Coffee Time";
            return View();
        }
    }
}
