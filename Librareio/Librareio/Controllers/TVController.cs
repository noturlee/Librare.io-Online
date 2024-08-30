using Microsoft.AspNetCore.Mvc;

namespace Librareio.Controllers
{
    public class TVController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
