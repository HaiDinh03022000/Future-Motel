using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace DataAccess
{
    public class OrderDAO
    {
        private static OrderDAO? instance;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }
        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    instance ??= new OrderDAO();
                    return instance;
                }
            }
        }

        public IEnumerable<Order> GetAll()
        {
            using var context = new FStoreContext();
            List<Order> list = context.Orders
                .Include(i => i.Member)
                .ToList();
            return list;
        }

        public Order? GetById(int id)
        {
            using var context = new FStoreContext();
            Order? order = context.Orders
                .Include(i => i.Member)
                .SingleOrDefault(m => m.OrderId == id);
            return order;
        }

        public void Add(Order order)
        {
            if (GetById(order.OrderId) != null)
                throw new Exception("Order has existed");
            using var context = new FStoreContext();
            context.Orders.Add(order);
            context.SaveChanges();
        }

        public void Update(Order order)
        {
            if (GetById(order.OrderId) == null)
                throw new Exception("Order does not exist");
            using var context = new FStoreContext();
            context.Orders.Update(order);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            Order? order = GetById(id);
            if (order == null)
                throw new Exception("Order does not exist");
            using var context = new FStoreContext();
            context.Orders.Remove(order);
            context.SaveChanges();
        }

        public IEnumerable<Order> GetByMemberId(int id)
        {
            using var context = new FStoreContext();
            return context.Orders
                .Include(i => i.Member)
                .Where(o => o.MemberId == id)
                .ToList();
        }
    }
}