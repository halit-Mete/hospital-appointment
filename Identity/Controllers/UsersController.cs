using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Identity.Controllers
{
    //[Authorize(Roles = "admin")] // Giriş yapmanın yanında admin de olman gerekiyor
    public class UsersController : Controller
    {
        private UserManager<AppUser> _userManager;

        private RoleManager<AppRole> _roleManager;
        public UsersController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // [AllowAnonymous] yukarıda hepsini kapattıktan sonra bu kod ile girişyapmadanda bunun gözükmesini sağlayabiliyoruz
        // aynı anda hem authorize hem AllowAnonymous kullanırsak her yer kapalı sadece AllowAnanymous açık
        // ama böyle yapıca aşağıdaki gibi TempData yollayamıyoruz onu öğren
        public IActionResult Index()
        {
            
            if (!User.IsInRole("admin")) // YETKİLİ GİRİŞ YAPILMAZSA GİRİŞ SAYFASINA YÖNLENDİRİP UYARI MESAJI VERİYOR
            {
                TempData["message"] = "Giriş yapınız";
                return RedirectToAction("Login", "Account");
            }
            
            return View(_userManager.Users);
        }

		public IActionResult Create()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if(ModelState.IsValid)
            {
                // UserName kullanmasak bile set etmemiz şart
                var user = new AppUser { UserName = model.UserName,
                    Email = model.Email, 
                    FullName = model.FullName};
                // var user = new IdentityUser

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
			if (!User.IsInRole("admin")) // YETKİLİ GİRİŞ YAPILMAZSA GİRİŞ SAYFASINA YÖNLENDİRİP UYARI MESAJI VERİYOR
			{
				TempData["message"] = "Yetkisiz giriş yaptınız. Bu girişi sadece yetkili hesaplar yapabilir. Hesaptan çıkıp yetkili bir hesapla giriş yapınız";
				return RedirectToAction("Login", "Account");
			}

			if (id == null)
            {
                ViewBag.msj = "Gecerli Id bulunamadı";
                return RedirectToAction("Index");
            }

            var user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();

                return View(new EditViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    SelectedRoles = await _userManager.GetRolesAsync(user)
                });
            }
            ViewBag.msj = "Gecerli Id bulunamadı";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, EditViewModel model)
        {
			if (!User.IsInRole("admin")) // YETKİLİ GİRİŞ YAPILMAZSA GİRİŞ SAYFASINA YÖNLENDİRİP UYARI MESAJI VERİYOR
			{
				TempData["message"] = "Giriş yapınız";
				return RedirectToAction("Login", "Account");
			}

			if ( id != model.Id)
            {
                return RedirectToAction("Index");
            }

            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if(user != null)
                {
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.FullName = model.FullName;

                    var result = await _userManager.UpdateAsync(user);

                    if(result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    { 
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, model.Password);
                    }

                    if(result.Succeeded)
                    {
                        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

                        if (model.SelectedRoles != null)
                        {
                            await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                        }

                        return RedirectToAction("Index");
                    }

                    foreach(IdentityError err in result.Errors) 
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}
