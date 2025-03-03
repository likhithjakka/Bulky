using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICatergoryRepository Catergory{ get; private set; }

        public UnitOfWork(ApplicationDbContext db) 
        {
            _db = db;
            Catergory = new CategoryRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();  
        }
    }
}
