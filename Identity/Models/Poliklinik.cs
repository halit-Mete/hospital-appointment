using System.ComponentModel.DataAnnotations;

namespace Identity.Models
{
    public class Poliklinik
    {
        [Key]
        public int PoliklinikId { get; set; }

        [Display(Name = "Poliklinik Adı")]
        public string? PoliklinikAd { get; set; }

        public List<Doktor> Doktorlar { get; set; } = new List<Doktor>();
    }
}
