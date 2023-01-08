using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using RaidBookStore.DataAccess;
using RaidBookStore.DataAccess.Repository.IRepository;
using RaidBookStore.Models;

namespace RaidBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                //Custom Error Message
                ModelState.AddModelError("name", "Display order cannot be same with Name value.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction("Index");

            }
            return View(category);
        }

        //EDIT ------------------------------
        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var categoryFind = _db.Categories.Find(id);

            var categoryFirst = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            //var categorySingle = _db.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryFirst == null)
            {
                return NotFound();
            }

            return View(categoryFirst);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                //Custom Error Message
                ModelState.AddModelError("name", "Display order cannot be same with Name value.");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category updated successfully!";
                return RedirectToAction("Index");

            }
            return View(category);
        }


        //DELETE ------------------------------
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var categoryFind = _db.Categories.Find(id);

            var categoryFirst = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            //var categorySingle = _db.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryFirst == null)
            {
                return NotFound();
            }

            return View(categoryFirst);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["Success"] = "Category deleted successfully!";
            return RedirectToAction("Index");

        }




    }
}
