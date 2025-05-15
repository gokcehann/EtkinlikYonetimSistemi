using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public class EtkinlikService : IEtkinlikService
    {
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly IIlgiAlaniRepository _ilgiAlaniRepository;
        private readonly IHavaDurumuService _havaDurumuService;

        public EtkinlikService(
            IEtkinlikRepository etkinlikRepository,
            IIlgiAlaniRepository ilgiAlaniRepository,
            IHavaDurumuService havaDurumuService)
        {
            _etkinlikRepository = etkinlikRepository;
            _ilgiAlaniRepository = ilgiAlaniRepository;
            _havaDurumuService = havaDurumuService;
        }

        public async Task<List<EtkinlikSonucDto>> GetAllAsync()
        {
            var etkinlikler = await _etkinlikRepository.GetAllAsync();
            var etkinlikDtos = new List<EtkinlikSonucDto>();

            foreach (var etkinlik in etkinlikler)
            {
                var dto = MapToDto(etkinlik);
                dto.HavaDurumu = await _havaDurumuService.GetHavaDurumuAsync(etkinlik.Tarih, "Istanbul"); // Varsayılan olarak İstanbul
                etkinlikDtos.Add(dto);
            }

            return etkinlikDtos;
        }

        public async Task<EtkinlikSonucDto> GetByIdAsync(int id)
        {
            var etkinlik = await _etkinlikRepository.GetByIdAsync(id);
            if (etkinlik == null)
                return new EtkinlikSonucDto { Basarili = false, Mesaj = "Etkinlik bulunamadı." };

            var dto = MapToDto(etkinlik);
            dto.HavaDurumu = await _havaDurumuService.GetHavaDurumuAsync(etkinlik.Tarih, "Istanbul"); // Varsayılan olarak İstanbul
            return dto;
        }

        public async Task<List<EtkinlikSonucDto>> GetByKategoriAsync(int kategoriId)
        {
            var etkinlikler = await _etkinlikRepository.GetByKategoriAsync(kategoriId);
            var etkinlikDtos = new List<EtkinlikSonucDto>();

            foreach (var etkinlik in etkinlikler)
            {
                var dto = MapToDto(etkinlik);
                dto.HavaDurumu = await _havaDurumuService.GetHavaDurumuAsync(etkinlik.Tarih, "Istanbul"); // Varsayılan olarak İstanbul
                etkinlikDtos.Add(dto);
            }

            return etkinlikDtos;
        }

        private EtkinlikSonucDto MapToDto(Etkinlik etkinlik)
        {
            return new EtkinlikSonucDto
            {
                EtkinlikId = etkinlik.EtkinlikId,
                Ad = etkinlik.Ad,
                Aciklama = etkinlik.Aciklama,
                Tarih = etkinlik.Tarih,
                Kapasite = etkinlik.Kapasite,
                KalanBilet = etkinlik.KalanBilet,
                KategoriId = etkinlik.KategoriId,
                KategoriAdi = etkinlik.IlgiAlani?.Ad,
                BiletFiyati = etkinlik.Biletler.FirstOrDefault()?.Fiyat ?? 0,
                Basarili = true
            };
        }

        public async Task<EtkinlikSonucDto> CreateAsync(EtkinlikOlusturDto dto)
        {
            var kategori = await _ilgiAlaniRepository.GetByIdAsync(dto.KategoriId);
            if (kategori == null)
                return new EtkinlikSonucDto { Basarili = false, Mesaj = "Kategori bulunamadı." };

            var etkinlik = new Etkinlik
            {
                Ad = dto.Ad,
                Aciklama = dto.Aciklama,
                Tarih = dto.Tarih,
                Kapasite = dto.Kapasite,
                KalanBilet = dto.Kapasite,
                KategoriId = dto.KategoriId,

                Biletler = new List<Bilet>
                {

                }
            };

            await _etkinlikRepository.AddAsync(etkinlik);
            return new EtkinlikSonucDto { Basarili = true, Mesaj = "Etkinlik başarıyla oluşturuldu.", Id = etkinlik.EtkinlikId };
        }

        public async Task<EtkinlikSonucDto> UpdateAsync(EtkinlikGuncelleDto dto)
        {
            var etkinlik = await _etkinlikRepository.GetByIdAsync(dto.EtkinlikId);
            if (etkinlik == null)
                return new EtkinlikSonucDto { Basarili = false, Mesaj = "Etkinlik bulunamadı." };

            var kategori = await _ilgiAlaniRepository.GetByIdAsync(dto.KategoriId);
            if (kategori == null)
                return new EtkinlikSonucDto { Basarili = false, Mesaj = "Kategori bulunamadı." };

            etkinlik.Ad = dto.Ad;
            etkinlik.Aciklama = dto.Aciklama;
            etkinlik.Tarih = dto.Tarih;
            etkinlik.Kapasite = dto.Kapasite;
            etkinlik.KategoriId = dto.KategoriId;

            var bilet = etkinlik.Biletler.FirstOrDefault();
            if (bilet != null)
            {
                bilet.Fiyat = dto.BiletFiyati;
            }

            await _etkinlikRepository.UpdateAsync(etkinlik);
            return new EtkinlikSonucDto { Basarili = true, Mesaj = "Etkinlik başarıyla güncellendi." };
        }

        public async Task<EtkinlikSonucDto> DeleteAsync(int id)
        {
            var etkinlik = await _etkinlikRepository.GetByIdAsync(id);
            if (etkinlik == null)
                return new EtkinlikSonucDto { Basarili = false, Mesaj = "Etkinlik bulunamadı." };

            await _etkinlikRepository.DeleteAsync(id);
            return new EtkinlikSonucDto { Basarili = true, Mesaj = "Etkinlik başarıyla silindi." };
        }
    }
}