using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;

namespace CafeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        public OrderRepository _repo = new OrderRepository();
        public AccountRepository _accRepo = new AccountRepository();
        public IActionResult Index()
        {
            Account acc = _accRepo.GetByID(HttpContext.Session.GetInt32("AccountId") ?? 0) ?? new Account();
            if (acc.Id <= 0)
            {
                return Redirect("/Shop/Index");
            }
            DateTime date = DateTime.Now;
            ViewBag.FirstDay = new DateTime(date.Year, date.Month, 1);
            ViewBag.LastDay = ViewBag.FirstDay.AddMonths(1).AddDays(-1);
            return View();
        }
        public JsonResult GetAll( [FromBody] InputDto input)
        {
            input.dateStart = new DateTime(input.dateStart.Value.Year, input.dateStart.Value.Month, input.dateStart.Value.Day, 0, 0, 0);
            input.dateEnd = new DateTime(input.dateEnd.Value.Year, input.dateEnd.Value.Month, input.dateEnd.Value.Day, 23, 59, 59);
            List<Order> data = SQLHelper<Order>.ProcedureToList("spGetAllOrder", new string[] { "@Request", "@PageNumber", "@Status", "@DateStart", "@DateEnd" },
                                                                    new object[] { input.request, input.pageNumber, input.status, input.dateStart, input.dateEnd });

            PaginationDto totalCount = SQLHelper<PaginationDto>.ProcedureToModel("spGetAllTotalOrder", new string[] { "@Request", "@Status", "@DateStart", "@DateEnd" },
                                                                    new object[] { input.request, input.status, input.dateStart, input.dateEnd });

            return Json(new { data, totalCount });
        }

        public JsonResult GetDetail(int OrderId)
        {
            List<OrderDetailsDto> lst = SQLHelper<OrderDetailsDto>.ProcedureToList("spGetOrderDetails",
                                                                                    new string[] { "@OrderId" },
                                                                                    new object[] { OrderId });
            return Json(lst);
        }
        public JsonResult ChangeStatusOrder(int orderId, int status)
        {
            Order model = _repo.GetByID(orderId) ?? new Order();
            string statusText = status == 1 ? "giao" : (status == 2 ? "xác nhận" : "hủy");
            if (model.Id <= 0) return Json(new { status = 0, message = "Không thể tìm thấy đơn hàng!" });

            if (model.Status == status) return Json(new { status = 0, message = $"Đơn hàng đã được {statusText}!" });
            model.Status = status;
            _repo.Update(model);
            return Json(new { status = 1, message = $"Thành công!" });
        }
    }
}
