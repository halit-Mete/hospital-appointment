using System.ComponentModel.DataAnnotations;

namespace Identity.ViewModels
{
	public class EditViewModel
	{
		public string? Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string? UserName { get; set; }

        [Display(Name = "Tam Adı")]
        public string? FullName { get; set; }

		[EmailAddress]
		public string? Email { get; set; }


        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
		public string? Password { get; set; }


        [Display(Name = "Şifre Tekrar")]
        [DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Parola Eşleşmiyor")] // password ile karşılaştırıp aynı mı karşılaştırıyor
		public string? ConfirmPassword { get; set; }

		public IList<string>? SelectedRoles { get; set; }
	}
}
