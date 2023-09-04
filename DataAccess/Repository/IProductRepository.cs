using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        void Insert(Product member);
        void Update(Product member);
        void Remove(int id);
    }
}
