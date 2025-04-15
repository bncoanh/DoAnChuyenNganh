using CafeShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
