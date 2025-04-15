using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        public AccountRepository _accRepo = new AccountRepository();
        public IActionResult Index()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if (acc.Id <= 0)
            {
                return Redirect("/Shop/Index");
            }
            return View();
        }

        public IActionResult actionResult()
        {
            ViewBag.data = _accRepo.GetAll();
            return View();
        }
        public JsonResult GetAllNoPage()
        {

            return Json(_accRepo.GetAll());
        }
        [HttpGet]
        public JsonResult GetAll(string request = "", int pageNumber = 1)
        {
            List<AccountDto> data = SQLHelper<AccountDto>.ProcedureToList("spGetAllAccount", new string[] { "@PageNumber", "@Request" }, new object[] { pageNumber, request});
            PaginationDto totalCount = SQLHelper<PaginationDto>.ProcedureToModel("spGetAllTotalAccount", new string[] { "@Request" }, new object[] { request });

            return Json(new { data, totalCount });
        }
        public JsonResult GetById(int Id)
        {
            return Json(_accRepo.GetByID(Id));
        }

        public JsonResult CreateOrUpdate([FromBody] Account data)
        {
            bool isCheck = _accRepo.GetAll().Any(p => p.Id != data.Id && p.Email.ToLower().Equals(data.Email.ToLower()));
            if(isCheck)
            {
                return Json(new { status = 0, message = "Email đã bị trùng! Hãy tạo lại email!" });
            }

            Account model = _accRepo.GetByID(data.Id) ?? new Account();

            model.Email = data.Email;
            model.PassWord = data.PassWord;
            model.Role = data.Role;
            model.FullName = data.FullName;
            model.Gender = data.Gender;
            model.PhoneNumber = data.PhoneNumber;
            model.Address = data.Address;
            model.IsActive = data.IsActive;

            if (model.Id > 0) _accRepo.Update(model);
            else _accRepo.Create(model);

            return Json(new { status = 1, message = "Thành công!" });
        }

        public bool IsActive(int Id, int status)
        {
            Account acc = _accRepo.GetByID(Id);
            acc.IsActive = status;
            _accRepo.Update(acc);
            return true;
        }

        public bool ResetPassword(int Id)
        {
            Account acc = _accRepo.GetByID(Id);
            acc.PassWord = "1";
            _accRepo.Update(acc);
            return true;
        }

    }
}