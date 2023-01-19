using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using RaidBookStore.DataAccess;
using RaidBookStore.DataAccess.Repository.IRepository;
using RaidBookStore.Models;
using RaidBookStore.Models.ViewModels;

namespace RaidBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        //private readonly ApplicationDbContext _db;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }



        public IActionResult Index()
        {
            //IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View();
        }

        
        //INSERT-EDIT(UPSERT) ------------------------------
        //GET
        public IActionResult Upsert(int? id)
        {
            ProductViewModel productViewModel = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),

            };
            if (id == null || id == 0)
            {
                //Create Product
                //ViewBag.CategoryList = CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productViewModel);
                //Update Product
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel objProduct,IFormFile file)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (objProduct.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, objProduct.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    objProduct.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if(objProduct.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(objProduct.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(objProduct.Product);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Product created successfully!";
                return RedirectToAction("Index");

            }
            return View(objProduct);
        }


        ////DELETE ------------------------------
        ////GET
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

            

        //    var coverTypeFirst = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            

        //    if (coverTypeFirst == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(coverTypeFirst);
        //}

        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objProduct = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (objProduct == null)
            {
                return Json(new { success = false, message = "Error while Deleting!" });
            }
            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, objProduct.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(objProduct);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully!" });
        }
        #endregion
    }
}
