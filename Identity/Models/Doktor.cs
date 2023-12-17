using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Doktor
    {
        [Key]
        public int DoktorId { get; set; }

        [Display(Name = "Doktor Adı")]
        public string? DoktorAd { get; set; }

        public int PoliklinikId { get; set; }
        public Poliklinik? Poliklinik { get; set; }
    }
}
