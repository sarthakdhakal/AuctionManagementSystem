using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SEIIIAssignment.Models;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace SEIIIAssignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SEIIIContext _context;

        public HomeController(ILogger<HomeController> logger, SEIIIContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            RecurringJob.AddOrUpdate(() =>UpdateDatabase(),"* * * * *",TimeZoneInfo.Local);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
        public ActionResult GetData()
        {
            int admin = _context.Users.Where(x => x.Role == "Admin").Count();
            int client = _context.Users.Where(x => x.Role == "Client").Count();
         
            Ratio obj = new Ratio();
            obj.admin = admin;
            obj.client = client;
       

            return Json(obj,JsonRequestBehavior.AllowGet);
        }

        public class Ratio
        {
            public int admin { get; set; }
            public int client { get; set; }
        }

        public void UpdateDatabase()
        {
            List<Item> items = _context.Items.Where(i => i.EndDate <= DateTime.Now && i.SellingAmount== null).ToList();
            foreach (var item in items)
            {
                var amount  = _context.Bids.Where(b => b.ItemId == item.ItemId).Max(b=>b.Amount);
                var bidder = _context.Bids.Where(b => b.ItemId == item.ItemId && Equals(b.Amount, amount)).Select(b=>b.BidderId).ToList().LastOrDefault();
                item.BoughtbyId = bidder;
                item.SellingAmount = amount;
                _context.Update(item);
                _context.SaveChanges();
            }
        }
    }
}