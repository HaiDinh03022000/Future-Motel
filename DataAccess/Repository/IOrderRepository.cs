using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<Order> GetAll();
        Order? GetById(int id);
        IEnumerable<Order> GetByMemberId(int id);
        void Insert(Order member);
        void Update(Order member);
        void Remove(int id);
    }
}
