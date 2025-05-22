using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace EtkinlikYonetimSistemi.Web.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IHttpClientFactory _httpClientFactory;

        public KullaniciController(IKullaniciService kullaniciService, IHttpClientFactory httpClientFactory)
        {
            _kullaniciService = kullaniciService;
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Kullanici/Kayit
        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }

        // POST: /Kullanici/Kayit
        [HttpPost]
        public async Task<IActionResult> Kayit(KayitViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = model.Adapt<KullaniciKayitDto>();
            var sonuc = await _kullaniciService.KayitOlAsync(dto);

            if (!sonuc.Basarili)
            {
                ModelState.AddModelError("", sonuc.Mesaj);
                return View(model);
            }

            TempData["BasariMesaji"] = "Kayıt başarılı, onay bekleniyor.";
            return RedirectToAction("Giris", "Kullanici");
        }

        // GET: /Kullanici/Giris
        [HttpGet]
        public IActionResult Giris()
        {
            return View(new GirisViewModel());
        }

        // POST: /Kullanici/Giris
        [HttpPost]
        public async Task<IActionResult> Giris(GirisViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = model.Adapt<KullaniciGirisDto>();
            var sonuc = await _kullaniciService.GirisYapAsync(dto);

            if (!sonuc.Basarili)
            {
                ModelState.AddModelError("", sonuc.Mesaj);
                return View(model);
            }

            // Kullanıcı kimlik bilgilerini oluştur
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, "Kullanici")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                });

            if (sonuc.SifreYenilemeGerekli)
            {
                return RedirectToAction("SifreYenileme", "Kullanici");
            }

            return RedirectToAction("Anasayfa", "Home");
        }

        [HttpGet]
        public IActionResult SifreYenileme()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SifreYenileme(SifreYenilemeViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.YeniSifre != model.YeniSifreTekrar)
            {
                ModelState.AddModelError("", "Yeni şifreler uyuşmuyor!");
                return View(model);
            }

            var dto = new SifreDegistirDto
            {
                Email = model.Email,
                YeniSifre = model.YeniSifre
            };

            var sonuc = await _kullaniciService.SifreDegistirAsync(dto);

            if (sonuc.Basarili)
            {
                TempData["Basarili"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Anasayfa", "Home");
            }

            ModelState.AddModelError("", sonuc.Mesaj ?? "Şifre değiştirme işlemi başarısız oldu.");
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Giris");
        }
    }
}
