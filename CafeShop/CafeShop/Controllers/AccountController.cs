using CafeShop.Models.DTOs;
using CafeShop.Models;
using Microsoft.AspNetCore.Mvc;
using CafeShop.Reposiory;

namespace CafeShop.Controllers
{
    public class AccountController : Controller
    {
        public AccountRepository _accRepo = new AccountRepository();
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email = "", string password = "", string confirmPassword = "", string fullname = "")
        {
            bool isCheck = _accRepo.GetAll().Any(x => x.Email == email);
            if (confirmPassword != password)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                return View();
            }
            else if (isCheck)
            {
                ViewBag.Error = "Email đã được sử dụng!";
                return View();
            }
            else
            {
                Account newAcc = new Account()
                {
                    Email = email,
                    PassWord = password,
                    FullName = fullname,
                    Address = "",
                    Gender = 0,
                    PhoneNumber = "",
                    Role = 1,
                    IsActive = 1
                };
                await _accRepo.CreateAsync(newAcc);
                HttpContext.Session.SetInt32("AccountId", newAcc.Id);
                HttpContext.Session.SetInt32("AccountRole", newAcc.Role ?? 1);
                HttpContext.Session.SetString("FullName", newAcc.FullName ?? "");
                if (newAcc.Id > 0) return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
