using RaidBookStore.DataAccess.Repository.IRepository;
using RaidBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaidBookStore.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private ApplicationDbContext _db;

        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(OrderDetail objOD)
        {
            _db.OrderDetails.Update(objOD);
        }
    }
}
