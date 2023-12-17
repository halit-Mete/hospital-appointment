using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<AppUser> _userManager;

        private RoleManager<AppRole> _roleManager;

        private SignInManager<AppUser> _signInManager;
        public AccountController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                    if(result.Succeeded) // başarılı ise başarısızlıktaki kilitlenme olaylarını resetliyoruz
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);
                        await _userManager.SetLockoutEndDateAsync(user, null); // null diyerek hemen sıfırlanmasını sağlıyoruz

                        return RedirectToAction("Index", "Home");
                    }
                    else if(result.IsLockedOut) // kullanıcı girişi başarısızsa
                    {
                        var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                        var timeLeft = lockoutDate.Value - DateTime.UtcNow;
                        ModelState.AddModelError("", $"Hesabınız kilitlendi. Lütfen {timeLeft.Minutes} dakika {timeLeft.Seconds} saniye sonra tekrar deneyiniz.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Hatalı parola");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresiyle bir hesap bulunamadı");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout() // coockie yi silerek logout yaptırıyor
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
     
    }
}
