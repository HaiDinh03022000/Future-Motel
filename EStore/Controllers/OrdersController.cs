using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EStore.Controllers
{
    public class OrdersController : Controller
    {
        IOrderRepository orderRepository = new OrderRepository();
        IMemberRepository memberRepository = new MemberRepository();
        IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();

        // GET: OrdersController
        public ActionResult Index()
        {
            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
                return RedirectToAction("Index", "Login");
            if (memberId == 1)
                return View(orderRepository.GetAll());
            return View(orderRepository.GetByMemberId(memberId.Value));
        }

        // GET: OrdersController/Details/5
        public ActionResult Details(int id)
        {
            var order = orderRepository.GetById(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // GET: OrdersController/Create
        public ActionResult Create()
        {
            ViewBag.MemberId = GetMemberList();
            return View();
        }

        // POST: OrdersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            try
            {
                orderRepository.Insert(order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.MemberId = GetMemberList();
                ViewBag.Message = ex.Message;
                return View(order);
            }
        }

        // GET: OrdersController/Edit/5
        public ActionResult Edit(int id)
        {
            var order = orderRepository.GetById(id);
            if (order == null)
                return NotFound();

            ViewBag.MemberId = GetMemberList();
            return View(order);
        }

        // POST: OrdersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Order order)
        {
            try
            {
                order.OrderId = id;
                orderRepository.Update(order);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.MemberId = GetMemberList();
                ViewBag.Message = ex.Message;
                return View(order);
            }
        }

        // GET: OrdersController/Delete/5
        public ActionResult Delete(int id)
        {
            var order = orderRepository.GetById(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        // POST: OrdersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Order order)
        {
            try
            {
                orderRepository.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(order);
            }
        }

        public ActionResult Report(DateTime? from, DateTime? to)
        {
            if (from == null || to == null)
                return View();

            DateTime fromDate = from.Value;
            DateTime toDate = to.Value;

            IEnumerable<Order> orders = orderRepository.GetAll()
                .Where(o => fromDate.Date.CompareTo(o.OrderDate.Date) <= 0 && o.OrderDate.Date.CompareTo(toDate.Date) <= 0)
                .OrderByDescending(o => o.OrderDate);
            IEnumerable<OrderDetail> details = orderDetailRepository.GetBetweenDays(fromDate, toDate);

            ViewBag.Orders = orders.Count();
            ViewBag.SoldProducts = details.Sum(d => d.Quantity);
            ViewBag.Customers = orders.DistinctBy(o => o.MemberId).Count();
            ViewBag.Turnover = details.Sum(d => d.UnitPrice * d.Quantity);

            ViewBag.From = fromDate.ToString("yyyy-MM-dd");
            ViewBag.To = toDate.ToString("yyyy-MM-dd");
            return View(orders);
        }

        private IEnumerable<SelectListItem> GetMemberList()
        {
            return memberRepository.GetAll()
                .Select(m => new SelectListItem(m.Email, m.MemberId.ToString()));
        }
    }
}
