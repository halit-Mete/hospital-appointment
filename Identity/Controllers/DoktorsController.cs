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
    public class DoktorsController : Controller
    {
        private readonly RandevuContext _context;

        public DoktorsController(RandevuContext context)
        {
            _context = context;
        }

        // GET: Doktors
        public async Task<IActionResult> Index()
        {
            var randevuContext = _context.Doktorlar.Include(d => d.Poliklinik);
            return View(await randevuContext.ToListAsync());
        }

        // GET: Doktors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Doktorlar == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktorlar
                .Include(d => d.Poliklinik)
                .FirstOrDefaultAsync(m => m.DoktorId == id);
            if (doktor == null)
            {
                return NotFound();
            }

            return View(doktor);
        }

        // GET: Doktors/Create
        public IActionResult Create()
        {
            var poliklinikList = _context.Poliklinikler
                .Select(p => new { Id = p.PoliklinikId, Display = $"{p.PoliklinikAd} - {p.PoliklinikId}" })
                .ToList();

            ViewData["Poliklinikler"] = new SelectList(poliklinikList, "Id", "Display");

            return View();
        }


        // POST: Doktors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DoktorId,DoktorAd,PoliklinikId")] Doktor doktor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doktor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PoliklinikId"] = new SelectList(_context.Poliklinikler, "PoliklinikId", "PoliklinikId", doktor.PoliklinikId);
            return View(doktor);
        }

        // GET: Doktors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Doktorlar == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor == null)
            {
                return NotFound();
            }

            // Buraya ekleyeceğiniz kısım
            var poliklinikList = _context.Poliklinikler
                .Select(p => new { Id = p.PoliklinikId, Display = $"{p.PoliklinikAd} - {p.PoliklinikId}" })
                .ToList();

            ViewData["PoliklinikId"] = new SelectList(poliklinikList, "Id", "Display");
            // --------------------------------

            return View(doktor);
        }


        // POST: Doktors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoktorId,DoktorAd,PoliklinikId")] Doktor doktor)
        {
            if (id != doktor.DoktorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doktor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoktorExists(doktor.DoktorId))
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
            ViewData["PoliklinikId"] = new SelectList(_context.Poliklinikler, "PoliklinikId", "PoliklinikId", doktor.PoliklinikId);
            return View(doktor);
        }

        // GET: Doktors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Doktorlar == null)
            {
                return NotFound();
            }

            var doktor = await _context.Doktorlar
                .Include(d => d.Poliklinik)
                .FirstOrDefaultAsync(m => m.DoktorId == id);
            if (doktor == null)
            {
                return NotFound();
            }

            return View(doktor);
        }

        // POST: Doktors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Doktorlar == null)
            {
                return Problem("Entity set 'RandevuContext.Doktorlar'  is null.");
            }
            var doktor = await _context.Doktorlar.FindAsync(id);
            if (doktor != null)
            {
                _context.Doktorlar.Remove(doktor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoktorExists(int id)
        {
          return (_context.Doktorlar?.Any(e => e.DoktorId == id)).GetValueOrDefault();
        }
    }
}
