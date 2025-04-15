using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
