using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BulkyBook.Data;
using BulkyBook.Areas.Admin.Models;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypesController : Controller
    {
        private readonly ApplecationDbContext _context;

        public CoverTypesController(ApplecationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CoverTypes
        public async Task<IActionResult> Index()
        {
              return View(await _context.CoverTypes.ToListAsync());
        }

        // GET: Admin/CoverTypes/Details/5
        public async Task<IActionResult> Details(double? id)
        {
            if (id == null || _context.CoverTypes == null)
            {
                return NotFound();
            }

            var coverType = await _context.CoverTypes
                .FirstOrDefaultAsync(m => m.id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // GET: Admin/CoverTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CoverTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CoverName")] CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coverType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        // GET: Admin/CoverTypes/Edit/5
        public async Task<IActionResult> Edit(double? id)
        {
            if (id == null || _context.CoverTypes == null)
            {
                return NotFound();
            }

            var coverType = await _context.CoverTypes.FindAsync(id);
            if (coverType == null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        // POST: Admin/CoverTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(double id, [Bind("id,CoverName")] CoverType coverType)
        {
            if (id != coverType.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coverType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoverTypeExists(coverType.id))
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
            return View(coverType);
        }

        // GET: Admin/CoverTypes/Delete/5
        public async Task<IActionResult> Delete(double? id)
        {
            if (id == null || _context.CoverTypes == null)
            {
                return NotFound();
            }

            var coverType = await _context.CoverTypes
                .FirstOrDefaultAsync(m => m.id == id);
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        // POST: Admin/CoverTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(double id)
        {
            if (_context.CoverTypes == null)
            {
                return Problem("Entity set 'ApplecationDbContext.CoverTypes'  is null.");
            }
            var coverType = await _context.CoverTypes.FindAsync(id);
            if (coverType != null)
            {
                _context.CoverTypes.Remove(coverType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoverTypeExists(double id)
        {
          return _context.CoverTypes.Any(e => e.id == id);
        }
    }
}
