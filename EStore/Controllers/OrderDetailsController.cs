using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;

namespace EStore.Controllers
{
    [Route("[controller]")]
    public class OrderDetailsController : Controller
    {
        IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
        IProductRepository productRepository = new ProductRepository();

        // GET: OrderDetailsController
        [HttpGet("{orderId}")]
        public ActionResult Index(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View(orderDetailRepository.GetByOrderId(orderId));
        }

        // GET: OrderDetailsController/Details/5
        [HttpGet("{orderId}/Details/{productId}")]
        public ActionResult Details(int orderId, int productId)
        {
            var orderDetail = orderDetailRepository.GetById(orderId, productId);
            if (orderDetail == null)
                return NotFound();
            return View(orderDetail);
        }

        // GET: OrderDetailsController/Create
        [HttpGet("{orderId}/Create")]
        public ActionResult Create(int orderId)
        {
            ViewBag.ProductId = GetProductList();
            return View(new OrderDetail { OrderId = orderId });
        }

        // POST: OrderDetailsController/Create
        [HttpPost("{orderId}/Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int orderId, OrderDetail orderDetail)
        {
            try
            {
                orderDetail.OrderId = orderId;
                orderDetailRepository.Insert(orderDetail);
                return RedirectToAction(nameof(Index), new { orderId });
            }
            catch (Exception ex)
            {
                ViewBag.ProductId = GetProductList();
                ViewBag.Message = ex.Message;
                return View(orderDetail);
            }
        }

        // GET: OrderDetailsController/Edit/5
        [HttpGet("{orderId}/Edit/{productId}")]
        public ActionResult Edit(int orderId, int productId)
        {
            var orderDetail = orderDetailRepository.GetById(orderId, productId);
            if (orderDetail == null)
                return NotFound();
            return View(orderDetail);
        }

        // POST: OrderDetailsController/Edit/5
        [HttpPost("{orderId}/Edit/{productId}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int orderId, int productId, OrderDetail orderDetail)
        {
            try
            {
                orderDetail.OrderId = orderId;
                orderDetail.ProductId = productId;
                orderDetailRepository.Update(orderDetail);
                return RedirectToAction(nameof(Index), new { orderId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(orderDetail);
            }
        }

        // GET: OrderDetailsController/Delete/5
        [HttpGet("{orderId}/Delete/{productId}")]
        public ActionResult Delete(int orderId, int productId)
        {
            var orderDetail = orderDetailRepository.GetById(orderId, productId);
            if (orderDetail == null)
                return NotFound();
            return View(orderDetail);
        }

        // POST: OrderDetailsController/Delete/5
        [HttpPost("{orderId}/Delete/{productId}")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int orderId, int productId, OrderDetail orderDetail)
        {
            try
            {
                orderDetailRepository.Remove(orderId, productId);
                return RedirectToAction(nameof(Index), new { orderId });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(orderDetail);
            }
        }

        private IEnumerable<SelectListItem> GetProductList()
        {
            return productRepository.GetAll()
                .Select(p => new SelectListItem(p.ProductName, p.ProductId.ToString()));
        }
    }
}
