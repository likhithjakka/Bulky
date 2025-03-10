using Bulky.Models;
using Bulky.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAccess.Repository;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModel;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public ProductController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll().ToList();
                _UnitOfWork.Product.GetAll().ToList();

           

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            
            ProductVM productVM = new()
            {

                CategoryList = _UnitOfWork.Catergory.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                }),
                Product = new Product()
            };
            if(id == null || id==0)
            {
                return View(productVM);

            }
            else
            {
                productVM.Product = _UnitOfWork.Product.GET(u=> u.Id==id);
                return View(productVM);
        }

            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? File)
        {
           
            if (ModelState.IsValid)
            {
                _UnitOfWork.Product.Add(productVM.Product);
                _UnitOfWork.Save();
                TempData["Success"] = "Product created successfully";
                return RedirectToAction("index");
            }
            else
            {
                productVM.CategoryList = _UnitOfWork.Catergory.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.ID.ToString()
                });
                      return View(productVM);
            }
          
        }

       
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? ProductID = _UnitOfWork.Product.GET(u=>u.Id==id);
            if (ProductID == null)
            {
                return NotFound();
            }
            return View(ProductID);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _UnitOfWork.Product.GET(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }

            _UnitOfWork.Product.Remove(obj);
            _UnitOfWork.Save();
            TempData["Success"] = "Product deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
