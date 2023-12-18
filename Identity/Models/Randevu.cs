using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Randevu
    {
        public int RandevuId { get; set; }

        [Display(Name = "Hasta Adı")]
        public string? HastaAd { get; set; }

        [Display(Name = "Hasta TC")]
        public string? HastaTc { get; set; }

        [Display(Name = "Hasta Telefon")]
        public string? HastaTel { get; set; }

        [Display(Name = "Randevu Tarihi")]

        public int DoktorId { get; set; }
        public DateTime RandevuTarih { get; set; }
        public Doktor? Doktor { get; set; }
    }
}
