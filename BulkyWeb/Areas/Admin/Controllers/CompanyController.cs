using Bulky.Models;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModel;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utility;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
   // [Authorize (Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CompanyController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            
          
            if(id == null || id==0)
            {
                return View(new Company());

            }
            else
            {
                Company companyObj = _UnitOfWork.Company.GET(u=> u.ID==id);
                return View(companyObj);
        }

            
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
           
            if (ModelState.IsValid)
            {
              
                if(companyObj.ID==0)
                {
                    _UnitOfWork.Company.Add(companyObj);

                }    
                else
                {
                    _UnitOfWork.Company.Update(companyObj);
                }
              
                _UnitOfWork.Save();
                TempData["Success"] = "Company created successfully";
                return RedirectToAction("index");
            }
            else
            {
                      return View(companyObj);
            }
          
        }
        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _UnitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _UnitOfWork.Company.GET(u => u.ID == id);

            if(CompanyToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error while detecting" });
            }

           
            _UnitOfWork.Company.Remove(CompanyToBeDeleted);
            _UnitOfWork.Save();

            return Json(new { success = true, Message = "Delete successful" });
        }

        #endregion
    }
    }
