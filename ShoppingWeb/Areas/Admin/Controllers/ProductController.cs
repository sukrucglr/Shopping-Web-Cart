using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingWeb.DataAccess.Repositories;
using ShoppingWeb.DataAccess.ViewModels;
using ShoppingWeb.Models;
using System.Data;

namespace ShoppingWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] 
    public class ProductController : Controller
    {
        private IUnitOfWork _unitofWork;
        private IWebHostEnvironment _hostingEnvironment;

        public ProductController(IUnitOfWork unitofWork, IWebHostEnvironment hostingEnvironment)
        {
            _unitofWork = unitofWork;
            _hostingEnvironment = hostingEnvironment;
        }
        #region APICALL
        public IActionResult AllProducts()
        {
            var products = _unitofWork.Product.GetAll(includeProperties: "Category");
            return Json(new { data = products });
        }

        #endregion

        public IActionResult Index()
        {
            ProductVM productVM = new ProductVM();
            productVM.Products = _unitofWork.Product.GetAll();
            return View();
        }
        //uy
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitofWork.Category.Add(category);
                _unitofWork.Save();
                TempData["Başarılı"] = "Kategori başarı ile kayıt edildi";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitofWork.Category.GetAll().Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitofWork.Product.GetT(x => x.Id == id);
                if (vm.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(ProductVM vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string fileName = string.Empty;
                if (file != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);
                    if (vm.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.Product.ImageUrl = @"\ProductImage\" + fileName;
                }
                if (vm.Product.Id == 0)
                {
                    _unitofWork.Product.Add(vm.Product);
                    TempData["Başarılı"] = "ürün kaydı başarılı";
                }
                else
                {
                    _unitofWork.Product.Update(vm.Product);
                    TempData["Başarılı"] = "Ürün güncelleme başarılı";
                }
                _unitofWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var category = _unitofWork.Category.GetT(x => x.Id == id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}

        #region DeleteAPICALL
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitofWork.Product.GetT(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false, message = "Data yüklenirken hata oluştu" });
            }
            else
            {
                var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitofWork.Product.Delete(product);
                _unitofWork.Save();
                return Json(new { success = true, message = "Ürün silindi" });
            }
        }
        #endregion

    }
}