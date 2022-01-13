using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId","Amount")] Bid bid)
        {
            if (ModelState.IsValid)
            {
                List<Bid> bids = _context.Bids.Where(b => b.ItemId == bid.ItemId).ToList();
                var estimatedAmount = _context.Items.Where(b => b.ItemId == bid.ItemId).Select(b=>b.EstimatedAmount).ToList().LastOrDefault();
                if (bids.Count>0)
                {
                    foreach (var bidData in bids)
                    {
                        if (bid.Amount> bidData.Amount )
                        {
                            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
                            bid.BidderId = Int32.Parse(userId);
                            bid.CreatedAt = DateTime.Now;
                            _context.Add(bid);
                            await _context.SaveChangesAsync();
                            return RedirectToAction("Details", "Items", new {id = bid.ItemId});
                        }
                        else
                        {
                            TempData["Message"] = "The bid amount is less than the previous bid";
                            return RedirectToAction("Details", "Items", new {id = bid.ItemId});
                        }
                }
            
                }
                else
                {
                    if (bid.Amount > estimatedAmount)
                    {
                        var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;

                        bid.BidderId = Int32.Parse(userId);
                        bid.CreatedAt = DateTime.Now;
                        _context.Add(bid);
                        await _context.SaveChangesAsync();


                        return RedirectToAction("Details", "Items", new {id = bid.ItemId});
                    }
                    else
                    {
                        TempData["Message"] = "The bid amount is less than the estimated amount";
                        return RedirectToAction("Details", "Items", new {id = bid.ItemId});
                    }
                }
              
               
            }

            return View(bid);
        }

    }
}
