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
    [Authorize (Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;   
        public ProductController(IUnitOfWork UnitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = UnitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeproperties:"Category").ToList();
         //       _UnitOfWork.Product.GetAll().ToList();

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
                string WWWRootPath = _WebHostEnvironment.WebRootPath;

                if (File != null)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                    string ProductPath = Path.Combine(WWWRootPath, @"Images\Product");

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var OldPath = Path.Combine( WWWRootPath,productVM.Product.ImageUrl.TrimStart('\\') );

                        if(System.IO.File.Exists(OldPath))
                        {
                            System.IO.File.Delete(OldPath);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(ProductPath, FileName), FileMode.Create))
                    { 
                    File.CopyTo(filestream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\"+FileName;
                }

                if(productVM.Product.Id==0)
                {
                    _UnitOfWork.Product.Add(productVM.Product);

                }    
                else
                {
                    _UnitOfWork.Product.Update(productVM.Product);
                }
              
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
        #region API CALLS 

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _UnitOfWork.Product.GetAll(includeproperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var ProductToBeDeleted = _UnitOfWork.Product.GET(u => u.Id == id);

            if(ProductToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error while detecting" });
            }

            var OldPath = Path.Combine(_WebHostEnvironment.WebRootPath, 
                ProductToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(OldPath))
            {
                System.IO.File.Delete(OldPath);
            }

            _UnitOfWork.Product.Remove(ProductToBeDeleted);
            _UnitOfWork.Save();

            return Json(new { success = true, Message = "Delete successful" });
        }

        #endregion
    }
    }
