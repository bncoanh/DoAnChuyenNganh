using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CafeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ProductRepository _productRepo = new ProductRepository();
        public AccountRepository _accRepo = new AccountRepository();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            ViewBag.Account = acc;

            List<ProductDto> data = SQLHelper<ProductDto>.ProcedureToList("spGetTop4BestSale", new string[] { "@topSale" }, new object[] {4});
            ViewBag.TopSeller = data;
            List<ProductDto> lst = SQLHelper<ProductDto>.ProcedureToList("spGetTop4ProductNew", new string[] { }, new object[] { });
            ViewBag.TopNew = lst;

            return View();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            HttpContext.Session.Remove("AccountId");
            HttpContext.Session.Remove("AccountRole");
            HttpContext.Session.Remove("FullName");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]        
        public IActionResult Privacy(string Email = "", string PassWord = "")
        {
            Account acc =_accRepo.GetAll().FirstOrDefault(x=>x.Email == Email && x.PassWord == PassWord) ?? new Account();
            if (acc.Id <= 0)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không chính xác!";
                return View();
            }
            if (acc.IsActive == 0)
            {
                ViewBag.Error = "Tài khoản đã bị khóa! Không thể đăng nhập!";
                return View();
            }
            HttpContext.Session.SetInt32("AccountId", acc.Id);
            HttpContext.Session.SetInt32("AccountRole", acc.Role ?? 1);
            HttpContext.Session.SetString("FullName", acc.FullName ?? "");
            return  RedirectToAction("Index", "Home");
        }
    }
}
