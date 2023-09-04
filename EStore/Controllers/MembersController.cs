using BusinessObject;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace EStore.Controllers
{
    public class MembersController : Controller
    {
        private IMemberRepository memberRepository = new MemberRepository();

        // GET: MembersController
        public ActionResult Index()
        {

            var memberId = HttpContext.Session.GetInt32("MemberId");
            if (memberId == null)
                return RedirectToAction("Index", "Login");
            if (memberId == 1)
                return View(memberRepository.GetAll());
            return View(memberRepository.GetAll().Where(m => m.MemberId == memberId));
        }

        // GET: MembersController/Details/5
        public ActionResult Details(int id)
        {
            string message = "Chào mừng bạn đến với trang chủ!";
            ViewData["1"] = message;
            var member = memberRepository.GetById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        // GET: MembersController/Create
        public ActionResult Create()
        {
            ViewBag.Country = GetCountryList();
            return View();
        }

        // POST: MembersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Member member)
        {
            try
            {
                if (ModelState.IsValid)
                    memberRepository.Insert(member);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Country = GetCountryList();
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        // GET: MembersController/Edit/5
        public ActionResult Edit(int id)
        {
            var member = memberRepository.GetById(id);
            if (member == null)
                return NotFound();

            ViewBag.Country = GetCountryList();
            return View(member);
        }

        // POST: MembersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Member member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    member.MemberId = id;
                    memberRepository.Update(member);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Country = GetCountryList();
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        // GET: MembersController/Delete/5
        public ActionResult Delete(int id)
        {
            var member = memberRepository.GetById(id);
            if (member == null)
                return NotFound();
            return View(member);
        }

        // POST: MembersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Member member)
        {
            try
            {
                memberRepository.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(member);
            }
        }

        private IEnumerable<SelectListItem> GetCountryList()
        {
            string path = @"countries.json";
            var data = System.IO.File.ReadAllText(path);
            var countries = JsonSerializer.Deserialize<Dictionary<string, string>>(data);
            return countries
                ?.Values
                ?.OrderBy(country => country)
                ?.Select(country => new SelectListItem(country, country))
                ?? new List<SelectListItem>(0);
        }
    }
}
