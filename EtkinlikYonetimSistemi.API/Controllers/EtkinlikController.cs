using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EtkinlikYonetimSistemi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EtkinlikController : ControllerBase
    {
        private readonly IEtkinlikService _etkinlikService;

        public EtkinlikController(IEtkinlikService etkinlikService)
        {
            _etkinlikService = etkinlikService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var etkinlikler = await _etkinlikService.GetAllAsync();
            return Ok(etkinlikler);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var etkinlik = await _etkinlikService.GetByIdAsync(id);
            if (etkinlik == null)
                return NotFound();

            return Ok(etkinlik);
        }

        [HttpGet("kategori/{kategoriId}")]
        public async Task<IActionResult> GetByKategori(int kategoriId)
        {
            var etkinlikler = await _etkinlikService.GetByKategoriAsync(kategoriId);
            return Ok(etkinlikler);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(EtkinlikOlusturDto dto)
        {
            var sonuc = await _etkinlikService.CreateAsync(dto);
            if (!sonuc.Basarili)
                return BadRequest(sonuc.Mesaj);

            return CreatedAtAction(nameof(GetById), new { id = sonuc.Id }, sonuc);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EtkinlikGuncelleDto dto)
        {
            if (id != dto.EtkinlikId)
                return BadRequest("ID'ler eşleşmiyor");

            var sonuc = await _etkinlikService.UpdateAsync(dto);
            if (!sonuc.Basarili)
                return BadRequest(sonuc.Mesaj);

            return Ok(sonuc);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _etkinlikService.DeleteAsync(id);
            if (!sonuc.Basarili)
                return BadRequest(sonuc.Mesaj);

            return Ok(sonuc);
        }
    }
}
