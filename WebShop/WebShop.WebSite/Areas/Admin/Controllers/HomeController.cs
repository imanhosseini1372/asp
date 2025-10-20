using Microsoft.AspNetCore.Mvc;

namespace WebShop.WebSite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {

        [Route("Admin/Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
