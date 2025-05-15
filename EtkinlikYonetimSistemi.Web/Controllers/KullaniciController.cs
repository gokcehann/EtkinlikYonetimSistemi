using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
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

            var dto = new KullaniciKayitDto
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Email = model.Email,
                Sifre = model.Sifre,
                IlgiAlaniIdleri = model.IlgiAlaniIdleri
            };

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
            return View();
        }

        // POST: /Kullanici/Giris
        [HttpPost]
        public async Task<IActionResult> Giris(KullaniciGirisDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7040/api/Kullanicilar/giris", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var sonuc = JsonConvert.DeserializeObject<GirisDto>(responseContent);

                Response.Cookies.Append("jwtToken", sonuc.Token.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddMinutes(30)
                });

                return RedirectToAction("Index", "Home");
            }
            else
            {
                var hata = await response.Content.ReadAsStringAsync();
                ViewBag.Hata = "Giriş başarısız: " + hata;
                return View(dto);
            }
        }
    }
}
