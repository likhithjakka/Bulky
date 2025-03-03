using Bulky.Models;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _UnitOfWork.Catergory.GetAll().ToList();

            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display Name and Display orderid is same");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Catergory.Add(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryID = _UnitOfWork.Catergory.GET(u => u.ID == id);
            if (categoryID == null)
            {
                return NotFound();
            }
            return View(categoryID);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {

            if (ModelState.IsValid)
            {
                _UnitOfWork.Catergory.Update(obj);
                _UnitOfWork.Save();
                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryID = _UnitOfWork.Catergory.GET(u=>u.ID==id);
            if (categoryID == null)
            {
                return NotFound();
            }
            return View(categoryID);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _UnitOfWork.Catergory.GET(u => u.ID == id);

            if (obj == null)
            {
                return NotFound();
            }

            _UnitOfWork.Catergory.Remove(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
