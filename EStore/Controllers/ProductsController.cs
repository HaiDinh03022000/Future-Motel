using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EStore.Controllers
{
    public class ProductsController : Controller
    {
        IProductRepository productRepository = new ProductRepository();

        // GET: ProductsController
        //public ActionResult Index()
        //{
        //    return View(productRepository.GetAll());
        //}

        public ActionResult Index(string? name, int? price)
        {
            SelectListItem[] priceRanges =
            {
                new SelectListItem("All", "0"),
                new SelectListItem("0 - 999,999", "1"),
                new SelectListItem("1,000,000 - 9,999,999", "2"),
                new SelectListItem("10,000,000 -", "3")
            };
            ViewBag.PriceRanges = priceRanges;

            IEnumerable<Product> products = productRepository.GetAll();
            if (name != null)
            {
                ViewBag.SearchName = name;
                return View(products.Where(p => p.ProductName
                    .Contains(name, StringComparison.OrdinalIgnoreCase)));
            }

            if (price != null)
            {
                priceRanges[price.Value].Selected = true;
                return View(products.Where(p =>
                {
                    return price.Value switch
                    {
                        1 => p.UnitPrice < 1_000_000,
                        2 => 1_000_000 <= p.UnitPrice && p.UnitPrice < 10_000_000,
                        3 => 10_000_000 <= p.UnitPrice,
                        _ => true,
                    };
                }));
            }

            return View(products);
        }

        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {
            var product = productRepository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // GET: ProductsController/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = GetCategoryIdList();
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                    productRepository.Insert(product);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.CategoryId = GetCategoryIdList();
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            var product = productRepository.GetById(id);
            if (product == null)
                return NotFound();

            ViewBag.CategoryId = GetCategoryIdList();
            return View(product);
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.ProductId = id;
                    productRepository.Update(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewBag.CategoryId = GetCategoryIdList();
                return View(product);
            }
        }

        // GET: ProductsController/Delete/5
        public ActionResult Delete(int id)
        {
            var product = productRepository.GetById(id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                productRepository.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }

        private IEnumerable<SelectListItem> GetCategoryIdList()
        {
            int n = 10;
            var list = new List<SelectListItem>(n);
            for (int i = 1; i <= n; i++)
            {
                list.Add(new SelectListItem(i.ToString(), i.ToString()));
            }
            return list;
        }
    }
}
