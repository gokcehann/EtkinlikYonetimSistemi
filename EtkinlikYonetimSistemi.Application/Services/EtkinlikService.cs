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
                dto.HavaDurumu = await _havaDurumuService.GetHavaDurumuAsync(etkinlik.Tarih, etkinlik.Sehir);
                KontrolEtkinlikPlanlanabilirligi(dto);
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
            dto.HavaDurumu = await _havaDurumuService.GetHavaDurumuAsync(etkinlik.Tarih, etkinlik.Sehir);
            KontrolEtkinlikPlanlanabilirligi(dto);
            return dto;
        }

        public async Task<List<EtkinlikSonucDto>> GetByKategoriAsync(int kategoriId)
        {
            var etkinlikler = await _etkinlikRepository.GetByKategoriAsync(kategoriId);
            var etkinlikDtos = new List<EtkinlikSonucDto>();

            foreach (var etkinlik in etkinlikler)
            {
                var dto = MapToDto(etkinlik);
                dto.HavaDurumu = await _havaDurumuService.GetHavaDurumuAsync(etkinlik.Tarih, etkinlik.Sehir);
                KontrolEtkinlikPlanlanabilirligi(dto);
                etkinlikDtos.Add(dto);
            }

            return etkinlikDtos;
        }

        private void KontrolEtkinlikPlanlanabilirligi(EtkinlikSonucDto dto)
        {
            if (dto.HavaDurumu == null)
            {
                dto.PlanlanabilirMi = true;
                dto.PlanlamaMesaji = $"Hava durumu bilgisi alınamadı, etkinlik planlanabilir. Şehir: {dto.Sehir}";
                return;
            }

            var olumsuzHavaKosullari = new[]
            {
                "Yağmurlu",
                "Sağanak Yağışlı",
                "Gök Gürültülü Fırtına",
                "Dolu ile Gök Gürültülü Fırtına",
                "Kar Yağışlı"
            };

            if (olumsuzHavaKosullari.Contains(dto.HavaDurumu.Durum) || dto.HavaDurumu.Sicaklik < -5)
            {
                dto.PlanlanabilirMi = false;
                dto.PlanlamaMesaji = $"{dto.Sehir} için hava koşulları uygun değil: {dto.HavaDurumu.Durum}, Sıcaklık: {dto.HavaDurumu.Sicaklik}°C";
            }
            else
            {
                dto.PlanlanabilirMi = true;
                dto.PlanlamaMesaji = $"{dto.Sehir} için hava koşulları uygun: {dto.HavaDurumu.Durum}, Sıcaklık: {dto.HavaDurumu.Sicaklik}°C";
            }
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
                Sehir = etkinlik.Sehir,
                Basarili = true
            };
        }

        public async Task<EtkinlikSonucDto> CreateAsync(EtkinlikOlusturDto dto)
        {
            try
            {
                var kategori = await _ilgiAlaniRepository.GetByIdAsync(dto.KategoriId);
                if (kategori == null)
                    return new EtkinlikSonucDto { Basarili = false, Mesaj = "Kategori bulunamadı." };

                // Eğer alt kategori ise, üst kategorinin fiyatını al
                decimal biletFiyati = kategori.BasePrice;
                if (!kategori.AnaKategoriMi && kategori.UstKategoriId.HasValue)
                {
                    var ustKategori = await _ilgiAlaniRepository.GetByIdAsync(kategori.UstKategoriId.Value);
                    if (ustKategori != null)
                    {
                        biletFiyati = ustKategori.BasePrice;
                    }
                }

                var etkinlik = new Etkinlik
                {
                    Ad = dto.Ad,
                    Aciklama = dto.Aciklama,
                    Tarih = dto.Tarih,
                    Kapasite = dto.Kapasite,
                    KalanBilet = dto.Kapasite,
                    KategoriId = dto.KategoriId,
                    Sehir = dto.Sehir,
                    BiletFiyati = biletFiyati,
                    Biletler = new List<Bilet>
                    {
                        new Bilet
                        {
                            Fiyat = biletFiyati,
                            StokAdedi = dto.Kapasite
                        }
                    }
                };

                await _etkinlikRepository.AddAsync(etkinlik);
                
                return new EtkinlikSonucDto 
                { 
                    Basarili = true, 
                    Mesaj = "Etkinlik başarıyla oluşturuldu.", 
                    Id = etkinlik.EtkinlikId,
                    Ad = etkinlik.Ad,
                    Aciklama = etkinlik.Aciklama,
                    Tarih = etkinlik.Tarih,
                    Kapasite = etkinlik.Kapasite,
                    KalanBilet = etkinlik.KalanBilet,
                    KategoriId = etkinlik.KategoriId,
                    BiletFiyati = etkinlik.BiletFiyati,
                    Sehir = etkinlik.Sehir
                };
            }
            catch (Exception ex)
            {
                return new EtkinlikSonucDto 
                { 
                    Basarili = false, 
                    Mesaj = $"Etkinlik oluşturulurken bir hata oluştu: {ex.Message}" 
                };
            }
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
            etkinlik.Sehir = dto.Sehir;

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