using BusinessObject;
using System.Diagnostics.Metrics;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO? instance;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    instance ??= new ProductDAO();
                    return instance;
                }
            }
        }

        public IEnumerable<Product> GetAll()
        {
            using var context = new FStoreContext();
            List<Product> list = context.Products.ToList();
            return list;
        }

        public Product? GetById(int id)
        {
            using var context = new FStoreContext();
            Product? product = context.Products.SingleOrDefault(m => m.ProductId == id);
            return product;
        }

        public void Add(Product product)
        {
            if (GetById(product.ProductId) != null)
                throw new Exception("Product has existed");
            using var context = new FStoreContext();
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void Update(Product product)
        {
            if (GetById(product.ProductId) == null)
                throw new Exception("Product does not exist");
            using var context = new FStoreContext();
            context.Products.Update(product);
            context.SaveChanges();
        }

        public void Remove(int id)
        {
            Product? product = GetById(id);
            if (product == null)
                throw new Exception("Product does not exist");
            using var context = new FStoreContext();
            context.Products.Remove(product);
            context.SaveChanges();
        }
    }
}