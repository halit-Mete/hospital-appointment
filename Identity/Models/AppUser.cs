using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    // Ekstra kullanıcı bilgisi alma
    public class AppUser: IdentityUser
    {
        public string? FullName { get; set; }

    }
}
