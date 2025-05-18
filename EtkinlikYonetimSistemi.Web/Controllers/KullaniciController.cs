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
            var model = new GirisViewModel();
            return View(model);
        }

        // POST: /Kullanici/Giris
        [HttpPost]
        public async Task<IActionResult> Giris(GirisViewModel dto)
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

                // JWT Token'ı cookie'ye yaz (Güvenlik için Secure ve SameSite ayarlarını yap)
                Response.Cookies.Append("jwtToken", sonuc.Token?.ToString() ?? string.Empty, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // https zorunluysa true, değilse false olabilir
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                });

                // Kullanıcı bilgilerini Claims ile ASP.NET'e tanıt
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, sonuc.Email),
                    new Claim(ClaimTypes.NameIdentifier, sonuc.KullaniciId.ToString()),
                    new Claim(ClaimTypes.Role, sonuc.Rol ?? "Kullanici")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    });

                // loginSayisi 0'dan büyükse anasayfaya, 0 ise şifre yenileme sayfasına yönlendir
                if (sonuc.LoginSayisi > 0)
                {
                    return RedirectToAction("Anasayfa", "Kullanici");
                }
                else
                {
                    return RedirectToAction("SifreYenileme", "Kullanici");
                }
            }
            else
            {
                var hata = await response.Content.ReadAsStringAsync();
                dynamic hataObj = JsonConvert.DeserializeObject(hata);
                string mesaj = hataObj?.mesaj ?? hata;
                ViewBag.Hata = "Giriş başarısız: " + mesaj;
                return View(dto);
            }
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

            // Kullanıcı ID'sini Claims'den al
            var kullaniciIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(kullaniciIdStr) || !int.TryParse(kullaniciIdStr, out int kullaniciId))
            {
                ModelState.AddModelError("", "Kullanıcı kimliği bulunamadı. Lütfen tekrar giriş yapın.");
                return View(model);
            }

            var dto = new SifreDegistirDto
            {
                KullaniciId = kullaniciId,
                Email = model.Email,
                YeniSifre = model.YeniSifre
            };

            // API'ya HTTP isteği gönder
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7040/api/Kullanicilar/sifre-degistir", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Basarili"] = "Şifre başarıyla değiştirildi.";
                return RedirectToAction("Anasayfa", "Home");
            }
            else
            {
                var hata = await response.Content.ReadAsStringAsync();
                dynamic hataObj = JsonConvert.DeserializeObject(hata);
                string mesaj = hataObj?.mesaj ?? hata;
                ModelState.AddModelError("", mesaj);
                return View(model);
            }
        }
    }
}
