using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Controllers
{
    [Authorize(Roles = "admin")]
    public class PolikliniksController : Controller
    {
        private readonly RandevuContext _context;

        public PolikliniksController(RandevuContext context)
        {
            _context = context;
        }

        // GET: Polikliniks
        public async Task<IActionResult> Index()
        {
              return _context.Poliklinikler != null ? 
                          View(await _context.Poliklinikler.ToListAsync()) :
                          Problem("Entity set 'RandevuContext.Poliklinikler'  is null.");
        }

        // GET: Polikliniks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Poliklinikler == null)
            {
                return NotFound();
            }

            var poliklinik = await _context.Poliklinikler
                .FirstOrDefaultAsync(m => m.PoliklinikId == id);
            if (poliklinik == null)
            {
                return NotFound();
            }

            return View(poliklinik);
        }

        // GET: Polikliniks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Polikliniks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PoliklinikId,PoliklinikAd")] Poliklinik poliklinik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(poliklinik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(poliklinik);
        }

        // GET: Polikliniks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Poliklinikler == null)
            {
                return NotFound();
            }

            var poliklinik = await _context.Poliklinikler.FindAsync(id);
            if (poliklinik == null)
            {
                return NotFound();
            }
            return View(poliklinik);
        }

        // POST: Polikliniks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PoliklinikId,PoliklinikAd")] Poliklinik poliklinik)
        {
            if (id != poliklinik.PoliklinikId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(poliklinik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PoliklinikExists(poliklinik.PoliklinikId))
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
            return View(poliklinik);
        }

        // GET: Polikliniks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Poliklinikler == null)
            {
                return NotFound();
            }

            var poliklinik = await _context.Poliklinikler
                .FirstOrDefaultAsync(m => m.PoliklinikId == id);
            if (poliklinik == null)
            {
                return NotFound();
            }

            return View(poliklinik);
        }

        // POST: Polikliniks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Poliklinikler == null)
            {
                return Problem("Entity set 'RandevuContext.Poliklinikler'  is null.");
            }
            var poliklinik = await _context.Poliklinikler.FindAsync(id);
            if (poliklinik != null)
            {
                _context.Poliklinikler.Remove(poliklinik);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PoliklinikExists(int id)
        {
          return (_context.Poliklinikler?.Any(e => e.PoliklinikId == id)).GetValueOrDefault();
        }
    }
}
