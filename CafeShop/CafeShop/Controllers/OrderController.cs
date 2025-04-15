using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Controllers
{
    public class OrderController : Controller
    {
        public AccountRepository _accRepo = new AccountRepository();
        public OrderRepository _repo = new OrderRepository();
        public OrderDetailsRepository _detailRepo = new OrderDetailsRepository();
        public IActionResult Index()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if (acc.Id <= 0)
            {
                return Redirect("/Shop/Index");
            }

            return View();
        }

        public IActionResult OrderDetails(int OrderId = 0)
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if (acc.Id <= 0)
            {
                return Redirect("/Shop/Index");
            }
            ViewBag.Order = _repo.GetByID(OrderId) ?? new Order();
            List<OrderDetailsDto> lst = SQLHelper<OrderDetailsDto>.ProcedureToList("spGetOrderDetails",
                                                                                    new string[] { "@OrderId" },
                                                                                    new object[] { OrderId });
            ViewBag.Details = lst;

            int totalMoney = 0;
            foreach (var item in lst)
            {
                totalMoney += item.TotalMoney;
            }
            ViewBag.Total = totalMoney;

            return View();
        }
        public JsonResult GetAll(string request = "")
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if (acc.Id <= 0)
            {
                return Json(new { status = 0, message = "Hãy đăng nhập để sử dụng tính năng này!" });
            }
            List<OrderDto> lst = SQLHelper<OrderDto>.ProcedureToList("spGetHistoryCheckOut",
                                                                      new string[] { "@AccountId", "@Request" },
                                                                      new object[] { acc.Id , request});
            foreach (var item in lst)
            {
                item.DateFormat = item.CreateDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
            return Json(new { status = 1, message = "", data = lst});
        }
        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] OrderDto data)
        {
            try
            {
                Account accout = _accRepo.GetByID(data.AccountId) ?? new Account();
                if (accout.Id <= 0) return Json(new { status = 0, message = "Đăng nhập để sử dụng tính năng này!" });
                if (data.Details == null || data.Details.Count <= 0) return Json(new { status = 0, message = "Hãy chọn ít nhất 1 sản phẩm để tạo đơn hàng!" });

                int currentYear = DateTime.Now.Year;
                List<Order> lst = SQLHelper<Order>.SqlToList($"SELECT * FROM [Order] WHERE YEAR(CreateDate) = {currentYear}");

                Order newOrder = new Order();

                newOrder.OrderCode = $"MHĐ{currentYear}CFS{lst.Count + 1}";
                newOrder.CustomerName = data.CustomerName;
                newOrder.PhoneNumber = data.PhoneNumber;
                newOrder.Address = data.Address;
                newOrder.Status = 0;
                newOrder.CreateDate = DateTime.Now;
                newOrder.CreateBy = accout.FullName;
                newOrder.AccountId = data.AccountId;

                await _repo.CreateAsync(newOrder);

                foreach (var item in data.Details)
                {
                    OrderDetail newOderDetails = new OrderDetail();
                    newOderDetails.Quantity = item.Quantity;
                    newOderDetails.TotalMoney = item.TotalMoney;
                    newOderDetails.ProductDetailId = item.ProductDetailId;
                    newOderDetails.OrderId = newOrder.Id;
                    _detailRepo.Create(newOderDetails);
                }

                SQLHelper<Cart>.SqlToModel($"DELETE FROM Cart WHERE AccountID = {accout.Id}");

                return Json(new { status = 1, message = "Đặt hàng thành công!" });
            }
            catch (Exception ex)
            {

                return Json(new { status = 0, message = ex.Message });
            }

        }
    }
}
