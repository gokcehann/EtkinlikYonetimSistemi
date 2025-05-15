using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using EtkinlikYonetimSistemi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Connection String tanımla (appsettings.json içinde tanımlı olduğuna emin ol)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. DbContext'i DI konteynerine ekle
builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Katmanlar arası servisleri bağla (DI)
builder.Services.AddScoped<IKullaniciRepository, KullaniciRepository>();
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<IIlgiAlaniRepository, IlgiAlaniRepository>();


// 4. Şifreleme servisi
builder.Services.AddScoped<IPasswordHasher<Kullanici>, PasswordHasher<Kullanici>>();

// 5. Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Kullanici/Giris";
        options.LogoutPath = "/Kullanici/Cikis";
        options.AccessDeniedPath = "/Kullanici/Giris";
        options.Cookie.Name = "EtkinlikYonetimi.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    });



builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.Secure = CookieSecurePolicy.Always;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});


// 6. MVC için controller ve view servisini ekle
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseCookiePolicy();

// 7. Orta katmanlar (middleware pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// 8. Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Kullanici}/{action=Giris}/{id?}");

app.Run();
