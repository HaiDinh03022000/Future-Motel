using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public IEnumerable<OrderDetail> GetBetweenDays(DateTime from, DateTime to) => OrderDetailDAO.Instance.GetBetweenDays(from, to);

        public OrderDetail? GetById(int orderId, int productId) => OrderDetailDAO.Instance.GetById(orderId, productId);

        public IEnumerable<OrderDetail> GetByOrderId(int orderId) => OrderDetailDAO.Instance.GetByOrderId(orderId);

        public void Insert(OrderDetail member) => OrderDetailDAO.Instance.Add(member);

        public void Remove(int orderId, int productId) => OrderDetailDAO.Instance.Remove(orderId, productId);

        public void Update(OrderDetail member) => OrderDetailDAO.Instance.Update(member);
    }
}
