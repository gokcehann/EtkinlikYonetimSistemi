using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Services;
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

                return BadRequest(new KayitDto
                {
                    Basarili = false,
                    Mesaj = string.Join(" | ", errors)
                });
            }

            var sonuc = await _kullaniciService.KayitOlAsync(dto);

            if (!sonuc.Basarili)
            {
                return BadRequest(sonuc);
            }

            return Ok(sonuc);
        }
    }
}
