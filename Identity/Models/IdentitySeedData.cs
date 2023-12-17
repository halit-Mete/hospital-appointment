using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models
{
    public class IdentitySeedData
    {
        private const string adminUser = "admin";

        private const string adminPassword = "b211210064";

        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IdentityContext>();

            // Bekleyen migration var mı / Varsa database update e gerek yok olanı aktarabiliriz
            if (context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }

            // Identity context üzerinden tool sınıfları var onlardan biri userManager hazır servis
            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            // Kullanıcı oluşturma, adminUser'a göre arayacak varsa bize identity user gelecek
            var user = await userManager.FindByNameAsync(adminUser);

            // Eğer bir user yoksa user oluştursun
            if (user == null)
            {
                user = new AppUser
                {
                    FullName = "Halit Mete Tunç",
                    UserName = adminUser,
                    Email = "b211210064@sakarya.edu.tr",
                    PhoneNumber = "5545"
                };
                // Kullanıcıyı oluştururken parola atıyor
                await userManager.CreateAsync(user, adminPassword);
                // sonradan passwordu verme nedenimiz hash'lenerek veritabanında saklanmasıdır (güvenlik için)
                // hackleyen kişi bu paraloyı göremez. Bu parolayı hash'leyen algoraitma parolayı valid edebilir
            }
        }

    }
}
