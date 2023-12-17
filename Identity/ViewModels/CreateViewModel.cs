using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
	public class CreateViewModel
	{
        [Display(Name = "Kullanıcı Adı")]
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "Tam Adı")]
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

        [Display(Name = "Şifre")]
        [Required]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

        [Display(Name = "Şifre Tekrar")]
        [Required]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Parola Eşleşmiyor")] // password ile karşılaştırıp aynı mı karşılaştırıyor
		public string ConfirmPassword { get; set; } = string.Empty;
    }
}
