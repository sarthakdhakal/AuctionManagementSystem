using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SEIIIAssignment.Models;

namespace SEIIIAssignment.Controllers
{
    public class ClassificationsController : Controller
    {
        private readonly SEIIIContext _context;

        public ClassificationsController(SEIIIContext context)
        {
            _context = context;
        }

        // GET: Classifications
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Classifications.ToListAsync());
        }

        // GET: Classifications/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications
                .FirstOrDefaultAsync(m => m.ClassificationId == id);
            if (classification == null)
            {
                return NotFound();
            }

            return View(classification);
        }

        // GET: Classifications/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ClassificationId,ClassificationName")] Classification classification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(classification);
        }

        // GET: Classifications/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classification = await _context.Classifications.FindAsync(id);
            if (classification == null)
            {
                return NotFound();
            }
            return View(classification);
        }

        // POST: Classifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClassificationId,ClassificationName")] Classification classification)
        {
            if (id != classification.ClassificationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(classification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassificationExists(classification.ClassificationId))
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
            return View(classification);
        }

        // GET: Classifications/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

         
            var classification = await _context.Classifications.FindAsync(id);
            if (classification== null)
            {
                return NotFound();
            }
            _context.Classifications.Remove(classification);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool ClassificationExists(int id)
        {
            return _context.Classifications.Any(e => e.ClassificationId == id);
        }
    }
}
