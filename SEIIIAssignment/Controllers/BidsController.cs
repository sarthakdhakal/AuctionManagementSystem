using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEIIIAssignment.Models;

namespace SEIIIAssignment.Controllers
{
    public class BidsController : Controller
    {
        private readonly SEIIIContext _context;

        public BidsController(SEIIIContext context)
        {
            _context = context;
        }

        // GET: Bids
       

        // GET: Bids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bid = await _context.Bids
                .Include(b => b.Bidder)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(m => m.BidId == id);
            if (bid == null)
            {
                return NotFound();
            }

            return View(bid);
        }

        // GET: Bids/Create
      

        // POST: Bids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BidId,ItemId,Amount")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                bid.CreatedAt = DateTime.Now;
                _context.Add(bid);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Items", new {id = bid.ItemId});
            }

            return View(bid);
        }

    }
}
