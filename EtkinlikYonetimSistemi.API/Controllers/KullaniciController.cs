using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EtkinlikYonetimSistemi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KullanicilarController : ControllerBase
    {
        private readonly IKullaniciService _kullaniciService;

        public KullanicilarController(IKullaniciService kullaniciService)
        {
            _kullaniciService = kullaniciService;
        }

        [HttpPost("kayit")]
        public async Task<IActionResult> KayitOl([FromBody] KullaniciKayitDto dto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { Basarili = false, Mesaj = string.Join(" | ", errors) });
            }

            var sonuc = await _kullaniciService.KayitOlAsync(dto);

            if (!sonuc.Basarili)
            {
                return BadRequest(sonuc);
            }

            Console.WriteLine($"Gelen Kullanıcı: {dto.Email} - {dto.Sifre}");
            return Ok(sonuc);
        }

        [HttpPost("giris")]
        public async Task<IActionResult> GirisYap([FromBody] KullaniciGirisDto dto)
        {
            var sonuc = await _kullaniciService.GirisYapAsync(dto);

            if (!sonuc.Basarili)
            {
                return BadRequest(sonuc);
            }

            return Ok(sonuc);
        }

        [HttpPost("sifre-degistir")]
        [AllowAnonymous]
        public async Task<IActionResult> SifreDegistir([FromBody] SifreDegistirDto dto)
        {
            var sonuc = await _kullaniciService.SifreDegistirAsync(dto);
            if (sonuc == null || !sonuc.Basarili)
            {
                return BadRequest(sonuc);
            }

            return Ok("Şifre başarıyla değiştirildi.");
        }

    }
}
