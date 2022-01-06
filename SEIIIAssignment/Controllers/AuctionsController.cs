using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEIIIAssignment.Models;

namespace SEIIIAssignment.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly SEIIIContext _context;

        public AuctionsController(SEIIIContext context)
        {
            _context = context;
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {

            var SEIIIContext = _context.Auctions.Include(a => a.Boughtby).Include(a => a.Postedby);
            return View(await SEIIIContext.ToListAsync());
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions
                .Include(a => a.Boughtby)
                .Include(a => a.Postedby)
                .FirstOrDefaultAsync(m => m.AuctionId == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        public IActionResult Create()
        {
            ViewData["BoughtbyId"] = new SelectList(_context.Users, "UserId", "UserNam");
            ViewData["PostedbyId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AuctionId,StartDate,EndDate,IsActive,CreatedAt,PostedbyId,BoughtbyId,ItemId,SellingAmount")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoughtbyId"] = new SelectList(_context.Users, "UserId", "UserId", auction.BoughtbyId);
            ViewData["PostedbyId"] = new SelectList(_context.Users, "UserId", "UserId", auction.PostedbyId);
            return View(auction);
        }

        // GET: Auctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            ViewData["BoughtbyId"] = new SelectList(_context.Users, "UserId", "UserId", auction.BoughtbyId);
            ViewData["PostedbyId"] = new SelectList(_context.Users, "UserId", "UserId", auction.PostedbyId);
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuctionId,StartDate,EndDate,IsActive,CreatedAt,PostedbyId,BoughtbyId,ItemId,SellingAmount")] Auction auction)
        {
            if (id != auction.AuctionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(auction.AuctionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BoughtbyId"] = new SelectList(_context.Users, "UserId", "UserId", auction.BoughtbyId);
            ViewData["PostedbyId"] = new SelectList(_context.Users, "UserId", "UserId", auction.PostedbyId);
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions
                .Include(a => a.Boughtby)
                .Include(a => a.Postedby)
                .FirstOrDefaultAsync(m => m.AuctionId == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.AuctionId == id);
        }
    }
}
