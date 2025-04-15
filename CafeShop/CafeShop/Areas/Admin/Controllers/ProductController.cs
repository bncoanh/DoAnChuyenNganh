using CafeShop.Models;
using CafeShop.Models.DTOs;
using CafeShop.Reposiory;
using ManagementCourse.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Configuration.Internal;
using System.Data;

namespace CafeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        ProductTypeRepository _proType = new ProductTypeRepository();
        ProductRepository _pro = new ProductRepository();
        ProductDetailsRepository _detailRepo = new ProductDetailsRepository();
        ProductImageRepository fileRepo = new ProductImageRepository();
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


        // ======================= Product=========================================================================

        public JsonResult GetAll(string request = "", int pageNumber = 1)
        {
            List<ProductDto> data = SQLHelper<ProductDto>.ProcedureToList("spGetAllProduct", new string[] { "@PageNumber", "@Request" }, new object[] { pageNumber, request });
            PaginationDto totalCount = SQLHelper<PaginationDto>.ProcedureToModel("spGetAllTotalProduct", new string[] { "@Request" }, new object[] { request });

            return Json(new { data, totalCount });
        }
        public async Task<JsonResult> CreateOrUpdate([FromBody] ProductDto data)
        {
            bool isCheck = _pro.GetAll().Where(p => p.Id != data.Id && p.Code == data.Code).Any();
            if (isCheck) return Json(new { status = 0, message = "Mã sản phẩm đã bị trùng!", result = 0 });

            //Lưu sản phẩm
            Product newPro = _pro.GetByID(data.Id) ?? new Product();

            newPro.Id = data.Id;
            newPro.Code = data.Code;
            newPro.Name = data.Name;
            newPro.IsActive = data.IsActive;
            newPro.Description = data.Description;
            newPro.ProductTypeId = data.ProductTypeId;

            if (newPro.Id > 0) _pro.Update(newPro);
            else await _pro.CreateAsync(newPro);

            //Lưu giá sản phẩm
            List<ProductDetails> listDel = SQLHelper<ProductDetails>.SqlToList($"Select * from ProductDetails where ProductId = {newPro.Id}");
            _detailRepo.Delete(listDel);

            foreach (ProductDetails item in data.ListDetails)
            {
                ProductDetails newDetail = new ProductDetails()
                {
                    Id = item.Id,
                    ProductId = newPro.Id,
                    ProductSizeId = item.ProductSizeId,
                    Price = item.Price
                };

                if (newDetail.Id > 0) _detailRepo.Update(newDetail);
                else _detailRepo.Create(newDetail);
            }

            return Json(new { status = 1, message = "Thành công!", result = newPro }); ;
        }


        public JsonResult GetById(int Id)
        {
            Product data = _pro.GetByID(Id);
            List<ProductDetails> details = SQLHelper<ProductDetails>.SqlToList($"Select * from ProductDetails where ProductId = {Id}");


            return Json(new { data, details });
        }

        public bool Delete(int Id)
        {
            _pro.Delete(Id);
            List<ProductDetails> listDel = SQLHelper<ProductDetails>.SqlToList($"Select * from ProductDetails where ProductId = {Id}");
            foreach (var item in listDel)
            {
                _detailRepo.Delete(item.Id);

            }
            return true;
        }

        // ======================= ProductType =========================================================================

        public IActionResult ProductType()
        {
            return View();
        }
        public JsonResult GetAllProductTypeNoPage()
        {
            return Json(_proType.GetAll().ToList());
        }
        [HttpGet]
        public JsonResult GetAllProductType(string request = "", int pageNumber = 1)
        {
            List<ProductType> data = SQLHelper<ProductType>.ProcedureToList("spGetAllProductType", new string[] { "@PageNumber", "@Request" }, new object[] { pageNumber, request });
            PaginationDto totalCount = SQLHelper<PaginationDto>.ProcedureToModel("spGetTotalCountProductType", new string[] { "@Request" }, new object[] { request });

            return Json(new { data, totalCount });
        }
        public JsonResult GetByIdProductType(int Id)
        {
            return Json(_proType.GetByID(Id));
        }
        public async Task<JsonResult> CreateProductType([FromBody] ProductType data)
        {

            bool isCheck = _proType.GetAll().Any(x => x.Id != data.Id && x.Code == data.Code);
            if (isCheck) return Json(new { status = 0, message = "Mã loại sản phẩm đã bị trùng! Hãy kiểm tra lại!", result = 0 });

            ProductType model = _proType.GetByID(data.Id) ?? new ProductType();

            model.Code = data.Code;
            model.Name = data.Name;
            model.GroupType = data.GroupType;
            model.Description = data.Description;

            if (model.Id > 0) _proType.Update(model);
            else await _proType.CreateAsync(model);


            return Json(new { status = 1, message = "", result = model });
        }

        public async Task<JsonResult> DeleteProductType(int Id)
        {

            bool isCheck = _pro.GetAll().Any(x => x.ProductTypeId == Id);
            if (isCheck) return Json(new { status = 0, message = "Mã loại sản phẩm đã được sử dụng! Không thể xóa!" });
            _proType.Delete(Id);
            return Json(new { status = 1, message = "" });
        }

        // ===================================================================  END =========================================================================

        [HttpPost]
        public async Task<IActionResult> UploadFile(int Id)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");
                var files = Request.Form.Files;
                List<ProductImage> listFiles = new List<ProductImage>();
                foreach (var file in files)
                {
                    if(file.Length <= 0) continue;
                    listFiles.Add(new ProductImage()
                    {
                        ImageUrl = timestamp + "_" + file.FileName,
                        FileName = file.FileName,
                        ProductId = Id,
                    });
                    string pathUpload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
                    string imagePath = pathUpload + $"\\{timestamp+ "_" + file.FileName}";
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                fileRepo.CreateRange(listFiles);
                return Ok(new { status = 1, message = "Upload file thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 1, message = ex.Message });
            }
        }

    }
}
