
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Classifications.ToListAsync());
        }

       
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


        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [Bind("ClassificationId,ClassificationName")] Classification classification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(classification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(classification);
        }

        
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

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,
            [Bind("ClassificationId,ClassificationName")] Classification classification)
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


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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