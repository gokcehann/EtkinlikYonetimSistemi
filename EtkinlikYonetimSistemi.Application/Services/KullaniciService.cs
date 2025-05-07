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

        public async Task<KayitDto> KayitOlAsync(KullaniciKayitDto dto)
        {
            var mevcutKullanici = await _kullaniciRepository.GetByEmailAsync(dto.Email);

            if (mevcutKullanici != null)
            {
                return new KayitDto { Basarili = false, Mesaj = "Bu email zaten kayıtlı." };
            }

            //eger e posta daha önce kullanılmamışsa devam et. eger kullanılmışsa mesaj gönder. 

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return new KayitDto { Basarili = false, Mesaj = "Email boş olamaz." };
            }
            if (string.IsNullOrWhiteSpace(dto.Sifre))
            {
                return new KayitDto { Basarili = false, Mesaj = "Şifre boş olamaz." };
            }
            if (dto.IlgiAlaniIdleri == null || !dto.IlgiAlaniIdleri.Any())
            {
                return new KayitDto { Basarili = false, Mesaj = "En az bir ilgi alanı seçmelisiniz." };
            }
            if (string.IsNullOrWhiteSpace(dto.Ad))
            {
                return new KayitDto { Basarili = false, Mesaj = "Ad boş olamaz." };
            }
            if (string.IsNullOrWhiteSpace(dto.Soyad))
            {
                return new KayitDto { Basarili = false, Mesaj = "Soyad boş olamaz." };
            }


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
                var ilgiAlanlari = await _ilgiAlaniRepository.GetByIdsAsync(dto.IlgiAlaniIdleri);
                yeniKullanici.IlgiAlanlari = ilgiAlanlari.ToList();
            }

            await _kullaniciRepository.AddAsync(yeniKullanici);

            //Sonuç döndür (ID dahil)
            return new KayitDto
            {
                Basarili = true,
                Mesaj = "Kayıt başarılı, onay bekleniyor.",
                Id = yeniKullanici.Id // Son eklenen kullanıcının ID'si
            };
        }


        public async Task<GirisDto> GirisYapAsync(KullaniciGirisDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) && string.IsNullOrWhiteSpace(dto.Sifre))
            {
                return new GirisDto { Basarili = false, Mesaj = "Email ve şifre boş olamaz." };
            }
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return new GirisDto { Basarili = false, Mesaj = "Email boş olamaz." };
            }
            if (string.IsNullOrWhiteSpace(dto.Sifre))
            {
                return new GirisDto { Basarili = false, Mesaj = "Şifre boş olamaz." };
            }
            var email = dto.Email.Trim().ToLower();
            var kullanici = await _kullaniciRepository.GetByEmailAsync(dto.Email);
            if (kullanici == null)
            {
                return new GirisDto { Basarili = false, Mesaj = "Kullanıcı bulunamadı." };
            }
            var dogrulamaSonucu = _passwordHasher.VerifyHashedPassword(kullanici, kullanici.SifreHash, dto.Sifre);
            if (dogrulamaSonucu != PasswordVerificationResult.Success)
            {
                return new GirisDto { Basarili = false, Mesaj = "Şifre yanlış." };
            }
            kullanici.LoginSayisi++;
            await _kullaniciRepository.UpdateAsync(kullanici);
            return new GirisDto
            {
                Basarili = true,
                Mesaj = "Giriş başarılı.",
                SifreYenilemeGerekli = kullanici.LoginSayisi <= 1
            };
        }
    }
}
