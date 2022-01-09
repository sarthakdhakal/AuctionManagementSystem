using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SEIIIAssignment.Models;

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
            RecurringJob.AddOrUpdate(() =>UpdateDatabase(),"1 0 * * *",TimeZoneInfo.Local);
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
        
        public void UpdateDatabase()
        {
            List<Item> items = _context.Items.Where(i => i.EndDate <= DateTime.Now).ToList();
            foreach (var item in items)
            {
                var amount  = item.Bids.Where(b => b.ItemId == item.ItemId).Max(b=>b.Amount);
                var bidder = item.Bids.Where(b => b.ItemId == item.ItemId && Equals(b.Amount, amount)).Select(b=>b.BidderId).ToList().LastOrDefault();
                item.BoughtbyId = bidder;
                item.SellingAmount = amount;
                _context.Update(item);
                _context.SaveChanges();


            }
        }
    }
}