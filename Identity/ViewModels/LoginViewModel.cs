using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; } = true;
    }
}
