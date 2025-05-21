using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public class DuyuruService : IDuyuruService
    {
        private readonly IDuyuruRepository _duyuruRepository;
        public DuyuruService(IDuyuruRepository duyuruRepository)
        {
            _duyuruRepository = duyuruRepository;
        }

        public async Task<List<DuyuruDto>> GetAllAsync()
        {
            var duyurular = await _duyuruRepository.GetAllAsync();
            return duyurular.Select(d => new DuyuruDto
            {
                DuyuruId = d.DuyuruId,
                Baslik = d.Baslik,
                Icerik = d.Icerik,
                OlusturmaTarihi = d.OlusturmaTarihi
            }).ToList();
        }

        public async Task<DuyuruDto> GetByIdAsync(int id)
        {
            var d = await _duyuruRepository.GetByIdAsync(id);
            if (d == null) return null;
            return new DuyuruDto
            {
                DuyuruId = d.DuyuruId,
                Baslik = d.Baslik,
                Icerik = d.Icerik,
                OlusturmaTarihi = d.OlusturmaTarihi
            };
        }

        public async Task<DuyuruDto> CreateAsync(DuyuruOlusturDto dto)
        {
            var duyuru = new Duyuru
            {
                Baslik = dto.Baslik,
                Icerik = dto.Icerik,
                OlusturmaTarihi = DateTime.Now
            };
            await _duyuruRepository.AddAsync(duyuru);
            return new DuyuruDto
            {
                DuyuruId = duyuru.DuyuruId,
                Baslik = duyuru.Baslik,
                Icerik = duyuru.Icerik,
                OlusturmaTarihi = duyuru.OlusturmaTarihi
            };
        }

        public async Task<DuyuruDto> UpdateAsync(DuyuruGuncelleDto dto)
        {
            var duyuru = await _duyuruRepository.GetByIdAsync(dto.DuyuruId);
            if (duyuru == null) return null;
            duyuru.Baslik = dto.Baslik;
            duyuru.Icerik = dto.Icerik;
            await _duyuruRepository.UpdateAsync(duyuru);
            return new DuyuruDto
            {
                DuyuruId = duyuru.DuyuruId,
                Baslik = duyuru.Baslik,
                Icerik = duyuru.Icerik,
                OlusturmaTarihi = duyuru.OlusturmaTarihi
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var duyuru = await _duyuruRepository.GetByIdAsync(id);
            if (duyuru == null) return false;
            await _duyuruRepository.DeleteAsync(id);
            return true;
        }
    }
}