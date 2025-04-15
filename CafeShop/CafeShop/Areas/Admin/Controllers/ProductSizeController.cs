using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductSizeController : Controller
    {
        ProductSizeRepository _sizeRepo = new ProductSizeRepository();
        AccountRepository _accRepo = new AccountRepository();
        public IActionResult Index()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0);
            if (acc == null || acc.Role != 2)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public JsonResult GetAllNoPage()
        {

            return Json(_sizeRepo.GetAll());
        } 
        public JsonResult GetAll(string request = "", int pageNumber = 1)
        {
            List<ProductSize> data = SQLHelper<ProductSize>.ProcedureToList("spGetAllProductSize", new string[] { "@PageNumber", "@Request" }, new object[] { pageNumber, request });
            PaginationDto totalCount = SQLHelper<PaginationDto>.ProcedureToModel("spGetAllTotalProductSize", new string[] { "@Request" }, new object[] { request });

            return Json(new { data, totalCount });
        }
        public JsonResult GetById(int Id)
        {
            return Json(_sizeRepo.GetByID(Id));
        }

        public bool CreateOrUpdate([FromBody] ProductSize data )
        {
            ProductSize model = _sizeRepo.GetByID(data.Id) ?? new ProductSize();    
            model.Name = data.Name;
            model.Description = data.Description;
            model.Code = data.Code;
            if (model.Id > 0) _sizeRepo.Update(model);
            else _sizeRepo.Create(model);
            return true;
        }

        public bool Delete(int Id)
        {
            _sizeRepo.Delete( Id );
            return true;
        }
    }
}
