using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Identity.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Identity.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly RandevuContext _context;

        public RandevuController(RandevuContext context)
        {
            _context = context;
        }

        // Randevu al kısmı
        public IActionResult RandevuAl()
        {
            var doktorlar = _context.Doktorlar
                .Include(d => d.Poliklinik)
                .Select(d => new SelectListItem
                {
                    Value = d.DoktorId.ToString(),
                    Text = $"{d.DoktorAd} - {(d.Poliklinik != null ? d.Poliklinik.PoliklinikAd : "Belirtilmemiş")}"
                })
                .ToList();

            ViewBag.Doktorlar = doktorlar;

            return View();
        }

        [HttpPost]
        public IActionResult RandevuAl(Randevu randevu)
        {
            if (ModelState.IsValid)
            {
                _context.Randevular.Add(randevu);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(randevu);
        }

        public IActionResult RandevuKontrol()
        {
            return View();
        }

        // Randevukontrol de aradığın tc ye göre seni randevugoster.cshtml'inde randevu bilgileri gözükmektedir
        [HttpPost]
        public IActionResult RandevuKontrol(string hastaTC)
        {
            var randevu = _context.Randevular
                .Include(r => r.Doktor)
                    .ThenInclude(d => d.Poliklinik)
                .FirstOrDefault(r => r.HastaTc == hastaTC);

            if (randevu != null)
            {
                return View("RandevuGoster", randevu);
            }
            else
            {
                ViewBag.Mesaj = "Belirtilen TC numarasına ait randevu bulunamadı.";
                return View();
            }
        }

        public IActionResult Randevular()
        {

            if (!User.IsInRole("admin")) // YETKİLİ GİRİŞ YAPILMAZSA GİRİŞ SAYFASINA YÖNLENDİRİP UYARI MESAJI VERİYOR
            {
                TempData["message"] = "Giriş yapınız";
                return RedirectToAction("Login", "Account");
            }

            var randevular = _context.Randevular
                .Include(r => r.Doktor)
                .ThenInclude(d => d.Poliklinik)
                .ToList();

            return View(randevular);
        }

        // Adminin yapabileceği silme işlemi
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var randevu = _context.Randevular.Find(id);

            if (randevu == null)
            {
                return NotFound();
            }

            _context.Randevular.Remove(randevu);
            _context.SaveChanges();

            return RedirectToAction("Randevular");
        }

    }
}
