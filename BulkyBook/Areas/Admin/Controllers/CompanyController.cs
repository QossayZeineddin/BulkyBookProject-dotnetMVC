using BulkyBook.Areas.Admin.Models;
using BulkyBook.Repository.IRepository;
using BulkyBook.Uitility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
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
 

        public IActionResult Upsert(double? id)
        {
            Company company = new();


            if (id == null || id == 0)
            {
                return View(company);
            }
            else
            {
                company = _unitOfWork.company.getFirstOrDefault(u => u.id == id);
                return View(company);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.id == 0)
                {
                    _unitOfWork.company.add(obj);
                    TempData["success"] = "company created successfully";

                }
                else
                {
                    _unitOfWork.company.update(obj);
                    TempData["success"] = "company Updated successfully";

                }

                _unitOfWork.save();
                return RedirectToAction("Index");
            }

            return View(obj);
        }



        #region API CALLS

        [HttpGet]
        public IActionResult getAll()
        {
            var CompanyList = _unitOfWork.company.getAll();
            return Json(new { data = CompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var comany = _unitOfWork.company.getFirstOrDefault(u => u.id == id);
            if (comany == null)
            {
                return Json(new { success = false, message = "Entity set 'Bulky.comany'  is null or there is a error" });
                //  return Problem("Entity set 'Bulky.products'  is null.");
            }

            _unitOfWork.company.remove(comany);
            _unitOfWork.save();
            return Json(new { success = true, message = "Delete Successfull" });
        }

        #endregion

    }

}








