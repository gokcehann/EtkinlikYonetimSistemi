using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Services;
using EtkinlikYonetimSistemi.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public class KullaniciService : IKullaniciService
    {
        //sifre hashleme islemi
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IIlgiAlaniRepository _ilgiAlaniRepository;
        private readonly PasswordHasher<Kullanici> _passwordHasher = new PasswordHasher<Kullanici>();

        public KullaniciService(IKullaniciRepository kullaniciRepository, IIlgiAlaniRepository ilgiAlaniRepository)
        {
            _kullaniciRepository = kullaniciRepository;
            _ilgiAlaniRepository = ilgiAlaniRepository;
        }

        public KayitDto KayitOl(KullaniciKayitDto dto)
        {
            var mevcutKullanici = _kullaniciRepository.GetByEmailAsync(dto.Email);
            if (mevcutKullanici != null)
            {
                return new KayitDto { Basarili = false, Mesaj = "Bu email zaten kayıtlı." };
            }
            //eger e posta daha önce kullanılmamışsa devam et. eger kullanılmışsa mesaj gönder. 

            var yeniKullanici = new Kullanici
            {
                Ad = dto.Ad,
                Soyad = dto.Soyad,
                Email = dto.Email,
                OnayliMi = false,
                Rol = true,
                LoginSayisi = 0
            };

            yeniKullanici.SifreHash = _passwordHasher.HashPassword(yeniKullanici, dto.Sifre); //sifrenin hashlenmis haliyle kaydettik

            if (dto.IlgiAlaniIdleri != null && dto.IlgiAlaniIdleri.Any())
            {
                // Ilgi alanı id'leri ile ilgili nesneleri veritabanından çek
                var ilgiAlanlari = _ilgiAlaniRepository.GetByIdsAsync(dto.IlgiAlaniIdleri); // Bu repository metodunu yazmalısın
                yeniKullanici.IlgiAlanlari = (ICollection<IlgiAlani>)ilgiAlanlari;
            }

            //Veritabanına ekle
            _kullaniciRepository.AddAsync(yeniKullanici);

            //Sonuç döndür (ID dahil)
            return new KayitDto
            {
                Basarili = true,
                Mesaj = "Kayıt başarılı, onay bekleniyor.",
                Id = yeniKullanici.Id // Son eklenen kullanıcının ID'si
            };

        }
    }
}
