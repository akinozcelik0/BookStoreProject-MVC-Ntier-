using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using RaidBookStore.DataAccess;
using RaidBookStore.Models;

namespace RaidBookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }



        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;
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
                _db.Categories.Add(category);
                _db.SaveChanges();
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

            var categoryFind = _db.Categories.Find(id);

            //var categoryFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            //var categorySingle = _db.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryFind == null)
            {
                return NotFound();
            }

            return View(categoryFind);
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
                _db.Categories.Update(category);
                _db.SaveChanges();
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

            var categoryFind = _db.Categories.Find(id);

            //var categoryFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            //var categorySingle = _db.Categories.SingleOrDefault(c => c.Id == id);

            if (categoryFind == null)
            {
                return NotFound();
            }

            return View(categoryFind);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var category = _db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            
            _db.Categories.Remove(category);
            _db.SaveChanges();
            TempData["Success"] = "Category deleted successfully!";
            return RedirectToAction("Index");

        }




    }
}
