using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Controllers
{
    public class CartController : Controller
    {
        public CartRepository _repo = new CartRepository();
        public AccountRepository _accRepo = new AccountRepository();
        public IActionResult Index()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if(acc.Id <= 0)
            {
                return Redirect("/Shop/Index");
            }
            ViewBag.Account = acc;
            return View();
        }
        public JsonResult GetCartByAccountId()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if (acc.Id <= 0)
            {
                return Json(new {status=0, massage="Đăng nhập để sử dụng tính năng!"});
            }
            List<CartDto> lst = SQLHelper<CartDto>.ProcedureToList("spGetCartByAccountId",
                                                                    new string[] { "@AccountId" },
                                                                    new object[] { acc.Id});
            if(lst.Count == 0) return Json(new { status = 2, massage = "Bạn chưa có sản phẩm nào trong giỏ hàng!"});

            return Json(new { status = 1, massage = "",data = lst });

        }
        public async Task<JsonResult> AddToCart(int productDetailId, int accountId, int quantity = 1)
        {
            try
            {
                if(accountId <= 0) return Json(new { status = 0, message = "Hãy đăng nhập để sử dụng tính năng này!" });
                if(productDetailId <= 0) return Json(new { status = 0, message = "Hãy chọn size sản phẩm!" });


                List<Cart> lst = SQLHelper<Cart>.SqlToList($"SELECT * FROM Cart WHERE AccountId = {accountId}");
                Cart model = lst.FirstOrDefault(x => x.ProductDetailId == productDetailId ) ?? new Cart();
                if (model.Id > 0)
                {
                    model.Quantity = model.Quantity + quantity;
                    _repo.Update(model);
                }
                else
                {
                    model.ProductDetailId = productDetailId;
                    model.AccountId = accountId;
                    model.Quantity = quantity;
                    await _repo.CreateAsync(model);
                }

                return Json(new { status = 1, message = "Thêm vào giỏ hàng thành công!"});
            }
            catch (Exception ex)
            {

                return Json(new { status = 0, message = ex.Message});
            }
            
        }


        public async Task<JsonResult> RemoveToCart(int cartId, int quantity = 1)
        {
            try
            {
                Cart model = _repo.GetByID(cartId) ?? new Cart();
                if (model.Id > 0)
                {
                    if(model.Quantity > quantity)
                    {
                        model.Quantity = model.Quantity - quantity;
                        _repo.Update(model);
                    }
                    else
                    {
                        _repo.Delete(model.Id);
                    }
                }
                else
                {
                    return Json(new { status = 0, message = "Không thể tìm thấy sản phẩm trong giỏ hàng!" });

                }

                return Json(new { status = 1, message = "Cập nhật giỏ hàng thành công!" });
            }
            catch (Exception ex)
            {

                return Json(new { status = 0, message = ex.Message });
            }

        }
    }
}
