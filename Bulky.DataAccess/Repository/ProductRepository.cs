using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
      

        public void Update(Product Obj)
        {
            //  _db.Products.Update(Obj);
            var objFromDB = _db.Products.FirstOrDefault(u => u.Id == Obj.Id);

            if(objFromDB!=null)
            {
                objFromDB.Title = Obj.Title;
                objFromDB.ISBN = Obj.ISBN;

                objFromDB.Price = Obj.Price;
                objFromDB.ListPrice = Obj.ListPrice;

                objFromDB.Price50 = Obj.Price50;
                objFromDB.Price100 = Obj.Price100;

                objFromDB.Description = Obj.Description;
                objFromDB.CategoryId = Obj.CategoryId;
                objFromDB.Author = Obj.Author;
                if(Obj.ImageUrl!=null)
                {
                    objFromDB.ImageUrl = Obj.ImageUrl;
                }
              

            }
        }

    }
}
