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

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
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
                //Update Product
            }

            

            return View(productViewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel objProduct,IFormFile file)
        {
            
            if (ModelState.IsValid)
            {
                //_unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                TempData["Success"] = "Cover Type updated successfully!";
                return RedirectToAction("Index");

            }
            return View(objProduct);
        }


        //DELETE ------------------------------
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            

            var coverTypeFirst = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            

            if (coverTypeFirst == null)
            {
                return NotFound();
            }

            return View(coverTypeFirst);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(coverType);
            _unitOfWork.Save();
            TempData["Success"] = "Cover Type deleted successfully!";
            return RedirectToAction("Index");

        }




    }
}
