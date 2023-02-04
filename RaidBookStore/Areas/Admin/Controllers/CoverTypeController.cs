using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using RaidBookStore.DataAccess;
using RaidBookStore.DataAccess.Repository.IRepository;
using RaidBookStore.Models;
using RaidBookStore.Utility;

namespace RaidBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]

    public class CoverTypeController : Controller
    {
        //private readonly ApplicationDbContext _db;

        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(coverType);
                _unitOfWork.Save();
                TempData["Success"] = "Cover Type created successfully!";
                return RedirectToAction("Index");

            }
            return View(coverType);
        }

        //EDIT ------------------------------
        //GET
        public IActionResult Edit(int? id)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(coverType);
                _unitOfWork.Save();
                TempData["Success"] = "Cover Type updated successfully!";
                return RedirectToAction("Index");

            }
            return View(coverType);
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
