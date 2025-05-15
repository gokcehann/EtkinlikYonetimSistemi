using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public class KullaniciService : IKullaniciService
    {
        //sifre hashleme islemi
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IIlgiAlaniRepository _ilgiAlaniRepository;
        private readonly PasswordHasher<Kullanici> _passwordHasher = new PasswordHasher<Kullanici>();
        private readonly IConfiguration _configuration;

        public KullaniciService(IKullaniciRepository kullaniciRepository, IIlgiAlaniRepository ilgiAlaniRepository, IConfiguration configuration)
        {
            _kullaniciRepository = kullaniciRepository;
            _ilgiAlaniRepository = ilgiAlaniRepository;
            _configuration = configuration;
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
                Rol = "Kullanici",
                LoginSayisi = 0,
                SifreHash = ""
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

        private string GenerateJwtToken(Kullanici kullanici)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, kullanici.Email),
                new Claim("id", kullanici.Id.ToString()),
                new Claim(ClaimTypes.Role, kullanici.Rol)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
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
            var kullanici = await _kullaniciRepository.GetByEmailAsync(email);
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
            var token = GenerateJwtToken(kullanici);

            return new GirisDto
            {
                Basarili = true,
                Mesaj = "Giriş başarılı.",
                Token = GenerateJwtToken(kullanici),
                SifreYenilemeGerekli = kullanici.LoginSayisi <= 1
            };
        }

        public async Task<KayitDto> SifreDegistirAsync(SifreDegistirDto dto)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(dto.KullaniciId);
            if (kullanici == null)
            {
                return new KayitDto { Basarili = false, Mesaj = "Kullanıcı bulunamadı." };
            }

            kullanici.SifreHash = _passwordHasher.HashPassword(kullanici, dto.YeniSifre);
            await _kullaniciRepository.UpdateAsync(kullanici);

            return new KayitDto { Basarili = true, Mesaj = "Şifre başarıyla değiştirildi." };
        }

        public async Task<bool> IlgiAlaniGuncelleAsync(IlgiAlaniGuncelle dto)
        {
            var kullanici = await _kullaniciRepository.GetByIdAsync(dto.Id);
            if (kullanici == null)
                return false;

            // Mevcut ilgi alanlarını sil
            kullanici.IlgiAlanlari.Clear();

            // Yeni ilgi alanlarını veritabanından çek ve ekle
            var ilgiAlanlari = await _ilgiAlaniRepository.GetByIdsAsync(dto.IlgiAlaniIdleri);
            foreach (var ilgiAlani in ilgiAlanlari)
            {
                kullanici.IlgiAlanlari.Add(ilgiAlani);
            }

            await _kullaniciRepository.UpdateAsync(kullanici);
            return true;
        }
    }
}
