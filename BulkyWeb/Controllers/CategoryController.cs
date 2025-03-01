using Bulky.Models;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace BulkyWeb.Controllers
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
            List<Category> objCategoryList = _db.Category.ToList();

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
            _db.Category.Add(obj);
            _db.SaveChanges();
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
            Category? categoryID = _db.Category.Find(id);
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
                _db.Category.Update(obj);
                _db.SaveChanges();
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
            Category? categoryID = _db.Category.Find(id);
            if (categoryID == null)
            {
                return NotFound();
            }
            return View(categoryID);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Category.Find(id);

            if (obj == null)
            {
                return NotFound();
            }

            _db.Category.Remove(obj);
            _db.SaveChanges();
            TempData["Success"] = "Category deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
