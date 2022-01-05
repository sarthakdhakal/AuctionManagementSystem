using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using SEIIIAssignment.Models;

namespace SEIIIAssignment.Controllers
{
    public class ItemsController : Controller
    {
        private IWebHostEnvironment _environment;
        private readonly SEIIIContext _context;

        public ItemsController(SEIIIContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var SEIIIContext = _context.Items.Include(i => i.Category).Include(i => i.Classification);
            return View(await SEIIIContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Classification)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["CategoryName"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["ClassificationName"] =
                new SelectList(_context.Classifications, "ClassificationId", "ClassificationName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "ItemId,ProducedYear,TextualDescription,Image,CreatedAt,Artist,ItemType,Material,Weight,Height,Length,Medium,IsFramed,Type,ProductName,CategoryName,ClassificationId")]
            Item item)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var image in files)
                {
                    if (image != null && image.Length > 0)
                    {
                        var file = image;
                        var uploads = Path.Combine(_environment.WebRootPath, "uploads\\img\\products");

                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") +
                                           Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                item.Image = file.FileName;
                            }
                        }
                    }
                }

                _context.Add(item);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryName"] =
                new SelectList(_context.Categories, "CategoryId", "CategoryName", item.CategoryId);
            ViewData["ClassificationName"] = new SelectList(_context.Classifications, "ClassificationId",
                "ClassificationId", item.ClassificationId);
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            ViewData["CategoryName"] = new SelectList(_context.Categories, "CategoryId", "CategoryName",
                item.Category.CategoryName);
            ViewData["ClassificationName"] = new SelectList(_context.Classifications, "ClassificationId",
                "ClassificationId", item.ClassificationId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "ItemId,ProducedYear,TextualDescription,Image,CreatedAt,Artist,ItemType,Material,Weight,Height,Length,Medium,IsFramed,Type,ProductName,CategoryId,ClassificationId")]
            Item item)
        {
            if (id != item.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.ItemId))
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

            ViewData["CategoryName"] =
                new SelectList(_context.Categories, "CategoryId", "CategoryName", item.CategoryId);
            ViewData["ClassificationName"] = new SelectList(_context.Classifications, "ClassificationId",
                "ClassificationName", item.ClassificationId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Category)
                .Include(i => i.Classification)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.ItemId == id);
        }
    }
}