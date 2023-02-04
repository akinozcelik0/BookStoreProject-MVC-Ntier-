using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using RaidBookStore.DataAccess;
using RaidBookStore.DataAccess.Repository.IRepository;
using RaidBookStore.Models;
using RaidBookStore.Models.ViewModels;
using RaidBookStore.Utility;

namespace RaidBookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]

    public class CompanyController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }



        public IActionResult Index()
        {
            return View();
        }

        
        //INSERT-EDIT(UPSERT) ------------------------------
        //GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
                
            }
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            
            if (ModelState.IsValid)
            {
                
                if(company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["Success"] = "Company created successfully!";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["Success"] = "Company updated successfully!";

                }
                _unitOfWork.Save();
                
                return RedirectToAction("Index");

            }
            return View(company);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        //POST
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var objCompany = _unitOfWork.Company.GetFirstOrDefault(c => c.Id == id);
            if (objCompany == null)
            {
                return Json(new { success = false, message = "Error while Deleting!" });
            }
            
            _unitOfWork.Company.Remove(objCompany);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully!" });
        }
        #endregion
    }
}
