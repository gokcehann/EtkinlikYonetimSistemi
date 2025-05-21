using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EtkinlikYonetimSistemi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DuyuruController : ControllerBase
    {
        private readonly IDuyuruService _duyuruService;
        public DuyuruController(IDuyuruService duyuruService)
        {
            _duyuruService = duyuruService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var duyurular = await _duyuruService.GetAllAsync();
            return Ok(duyurular);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var duyuru = await _duyuruService.GetByIdAsync(id);
            if (duyuru == null)
                return NotFound();
            return Ok(duyuru);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DuyuruOlusturDto dto)
        {
            var sonuc = await _duyuruService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = sonuc.DuyuruId }, sonuc);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DuyuruGuncelleDto dto)
        {
            if (id != dto.DuyuruId)
                return BadRequest("ID'ler eşleşmiyor");
            var sonuc = await _duyuruService.UpdateAsync(dto);
            if (sonuc == null)
                return NotFound();
            return Ok(sonuc);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sonuc = await _duyuruService.DeleteAsync(id);
            if (!sonuc)
                return NotFound();
            return NoContent();
        }
    }
}