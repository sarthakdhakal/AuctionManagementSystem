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

            return RedirectToAction("Login", "Account");
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
        public ActionResult AdHoc()
        {
             ViewData["Admin"] = _context.Users.Where(x => x.Role == "Admin").Count();
             ViewData["Client"] = _context.Users.Where(x => x.Role == "Client").Count();

             var platform_family =  _context.Items.OrderBy(b=>b.CategoryId).GroupBy(r=>r.CategoryId).Select(g=>new {CategoryId= g.Key}).ToList();
             var value= (from b in _context.Items
                 
                 
                 group b by b.CategoryId into g
                 select new { platform_family = g.Key }).ToList();
             decimal total = _context.Items.Count();
             List<string> platformfamily = new List<string>();
             List<string> percentages = new List<string>();
             foreach (var item in platform_family)
             {
                 decimal sum = _context.Items
                     .Where(x => x.CategoryId == item.CategoryId)
                     .OrderBy(x => x.CategoryId)
                     .Count();
                 var categoryName = _context.Items.Where(x => x.CategoryId == item.CategoryId)
                     .Select(x => x.Category.CategoryName).FirstOrDefault();

                 platformfamily.Add(categoryName);
                 percentages.Add(sum.ToString("0"));
             }
             TempData["platform_family"] = string.Join(",", platformfamily);
             TempData["percentage"] = string.Join(",", percentages);
             
             return View();
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