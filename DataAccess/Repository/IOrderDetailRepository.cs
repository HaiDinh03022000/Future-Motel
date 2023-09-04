using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetByOrderId(int orderId);
        OrderDetail? GetById(int orderId, int productId);
        IEnumerable<OrderDetail> GetBetweenDays(DateTime from, DateTime to);
        void Insert(OrderDetail member);
        void Update(OrderDetail member);
        void Remove(int orderId, int productId);
    }
}
