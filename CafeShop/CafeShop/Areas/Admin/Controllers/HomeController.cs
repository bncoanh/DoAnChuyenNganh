using Azure.Core;
using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        AccountRepository _accRepo = new AccountRepository();
        public IActionResult Index()
        {
            /*Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0);
            if (acc == null || acc.Role != 2)
            {
                return Redirect("/Home/Index");
            }*/
            ViewBag.Month = DateTime.Now.ToString("yyyy-MM");
            ViewBag.Year = DateTime.Now.ToString("yyyy");
            ViewBag.Month1 = DateTime.Now.ToString("MM");
            return View();
        }
        public JsonResult GetTopSale(int topSale)
        {
            List<ProductDto> data = SQLHelper<ProductDto>.ProcedureToList("spGetTop4BestSale", new string[] { "@topSale" }, new object[] { topSale });
            return Json(data);
        }

        public JsonResult GetHardestToSell(int topSale)
        {
            List<ProductDto> data = SQLHelper<ProductDto>.ProcedureToList("spGetHardestToSell", new string[] { "@topSale" }, new object[] { topSale });
            return Json(data);
        }


        public JsonResult GetPuchase(int month, int year)
        {
            List<PuchaseDto> data = SQLHelper<PuchaseDto>.ProcedureToList("spGetTotalPuchase", new string[] { "@Month", "@Year" }, new object[] { month, year });
            return Json(data);
        }
    }

}
