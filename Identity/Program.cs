using Identity.Models;
using IdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Identity islemleri (1)
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:SQLite_Connection"]));

//Randevu Context

builder.Services.AddDbContext<RandevuContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:Randevu_Connection"]));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IdentityContext>();
// builder.Services.AddIdentity<IdentityUser, IdentityRole> yerine Appleri yazdýk 

// Sýfre kurallarý (3)

builder.Services.Configure<IdentityOptions>(options =>
{
    // Sifre ayarlarý
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    options.User.RequireUniqueEmail = true;

    // kullanýcý ayarlarý
    options.User.AllowedUserNameCharacters ="abcçdefgðhýijklmnoöpqrsþtuvwxyz_-.";

    // hesap kilitlenme ayarlarý
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login"; // Eriþemediðin bir sayfada nereye yönlendirilsin
    options.SlidingExpiration = true; // 7. günde tekrar girerse 7 gün tekrar set edilir
    options.ExpireTimeSpan = TimeSpan.FromDays(7);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// (2)
IdentitySeedData.IdentityTestUser(app);

app.Run();
