using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<KullaniciService> _logger;

        public KullaniciService(IKullaniciRepository kullaniciRepository, IIlgiAlaniRepository ilgiAlaniRepository, IConfiguration configuration, ILogger<KullaniciService> logger)
        {
            _kullaniciRepository = kullaniciRepository;
            _ilgiAlaniRepository = ilgiAlaniRepository;
            _configuration = configuration;
            _logger = logger;
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

        public async Task<SifreDegistirSonucDto> SifreDegistirAsync(SifreDegistirDto dto)
        {
            try
            {
                _logger.LogInformation($"Şifre değiştirme işlemi başlatıldı. Email: {dto.Email}");

                if (string.IsNullOrWhiteSpace(dto.Email))
                {
                    _logger.LogWarning("Email adresi boş gönderildi.");
                    return new SifreDegistirSonucDto { Basarili = false, Mesaj = "Email adresi boş olamaz." };
                }

                if (string.IsNullOrWhiteSpace(dto.YeniSifre))
                {
                    _logger.LogWarning("Yeni şifre boş gönderildi.");
                    return new SifreDegistirSonucDto { Basarili = false, Mesaj = "Yeni şifre boş olamaz." };
                }

                var kullanici = await _kullaniciRepository.GetByEmailAsync(dto.Email.Trim().ToLower());
                if (kullanici == null)
                {
                    _logger.LogWarning($"Kullanıcı bulunamadı. Email: {dto.Email}");
                    return new SifreDegistirSonucDto { Basarili = false, Mesaj = "Bu email adresine sahip kullanıcı bulunamadı." };
                }

                _logger.LogInformation($"Kullanıcı bulundu. ID: {kullanici.Id}, Email: {kullanici.Email}");

                var eskiSifreHash = kullanici.SifreHash;
                kullanici.SifreHash = _passwordHasher.HashPassword(kullanici, dto.YeniSifre);
                
                _logger.LogInformation($"Yeni şifre hash'lendi. Eski hash: {eskiSifreHash}, Yeni hash: {kullanici.SifreHash}");

                try
                {
                    await _kullaniciRepository.UpdateAsync(kullanici);
                    _logger.LogInformation($"Kullanıcı güncellendi. ID: {kullanici.Id}, Yeni Hash: {kullanici.SifreHash}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Kullanıcı güncellenirken hata oluştu. ID: {kullanici.Id}");
                    throw;
                }

                return new SifreDegistirSonucDto { Basarili = true, Mesaj = "Şifre başarıyla değiştirildi." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Şifre değiştirme işlemi sırasında bir hata oluştu.");
                return new SifreDegistirSonucDto { Basarili = false, Mesaj = "Şifre değiştirme işlemi sırasında bir hata oluştu." };
            }
        }
    }
}
