using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace DataAccess
{
    public class OrderDetailDAO
    {
        private static OrderDetailDAO? instance;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }
        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    instance ??= new OrderDetailDAO();
                    return instance;
                }
            }
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            using var context = new FStoreContext();
            List<OrderDetail> list = context.OrderDetails
                .Include(I => I.Order)
                .Include(i => i.Product)
                .ToList();
            return list;
        }

        public OrderDetail? GetById(int orderId, int productId)
        {
            using var context = new FStoreContext();
            OrderDetail? orderDetail = context.OrderDetails
                .Include(I => I.Order)
                .Include(i => i.Product)
                .SingleOrDefault(m => m.OrderId == orderId && m.ProductId == productId);
            return orderDetail;
        }

        public void Add(OrderDetail orderDetail)
        {
            if (GetById(orderDetail.OrderId, orderDetail.ProductId) != null)
                throw new Exception("OrderDetail has existed");
            orderDetail.UnitPrice = ProductDAO.Instance
                .GetById(orderDetail.ProductId)!
                .UnitPrice;
            using var context = new FStoreContext();
            context.OrderDetails.Add(orderDetail);
            context.SaveChanges();
        }

        public void Update(OrderDetail orderDetail)
        {
            if (GetById(orderDetail.OrderId, orderDetail.ProductId) == null)
                throw new Exception("OrderDetail does not exist");
            orderDetail.UnitPrice = ProductDAO.Instance
                .GetById(orderDetail.ProductId)!
                .UnitPrice;
            using var context = new FStoreContext();
            context.OrderDetails.Update(orderDetail);
            context.SaveChanges();
        }

        public void Remove(int orderId, int productId)
        {
            OrderDetail? orderDetail = GetById(orderId, productId);
            if (orderDetail == null)
                throw new Exception("OrderDetail does not exist");
            using var context = new FStoreContext();
            context.OrderDetails.Remove(orderDetail);
            context.SaveChanges();
        }

        public IEnumerable<OrderDetail> GetBetweenDays(DateTime from, DateTime to)
        {
            using var context = new FStoreContext();
            return context.OrderDetails
                .Include(d => d.Order)
                .Include(d => d.Product)
                .Where(d => from.Date.CompareTo(d.Order.OrderDate.Date) <= 0 && d.Order.OrderDate.Date.CompareTo(to.Date) <= 0)
                .ToList();
        }

        public IEnumerable<OrderDetail> GetByOrderId(int orderId)
        {
            using var context = new FStoreContext();
            return context.OrderDetails
                .Include(I => I.Order)
                .Include(i => i.Product)
                .Where(d => orderId == d.OrderId)
                .ToList();
        }
    }
}