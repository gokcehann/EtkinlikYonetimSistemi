using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IIlgiAlaniRepository _ilgiAlaniRepository;
        private readonly IPasswordHasher<Kullanici> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly ILogger<KullaniciService> _logger;

        public KullaniciService(
            IKullaniciRepository kullaniciRepository,
            IIlgiAlaniRepository ilgiAlaniRepository,
            IPasswordHasher<Kullanici> passwordHasher,
            IConfiguration configuration,
            ILogger<KullaniciService> logger)
        {
            _kullaniciRepository = kullaniciRepository;
            _ilgiAlaniRepository = ilgiAlaniRepository;
            _passwordHasher = passwordHasher;
            _configuration = configuration;
            _logger = logger;
        }

        private string TokenOlustur(Kullanici kullanici)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key bulunamadı"));
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                    new Claim(ClaimTypes.Email, kullanici.Email),
                    new Claim(ClaimTypes.Role, kullanici.Rol)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"] ?? "60")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<KayitDto> KayitOlAsync(KullaniciKayitDto dto)
        {
            try
            {
                var mevcutKullanici = await _kullaniciRepository.GetByEmailAsync(dto.Email);
                if (mevcutKullanici != null)
                {
                    return new KayitDto { Basarili = false, Mesaj = "Bu email zaten kayıtlı." };
                }

                var yeniKullanici = new Kullanici
                {
                    Email = dto.Email,
                    Ad = dto.Ad,
                    Soyad = dto.Soyad,
                    OnayliMi = dto.Email.ToLower() == "admin@gmail.com",
                    Rol = dto.Email.ToLower() == "admin@gmail.com" ? "Admin" : "Kullanici",
                    LoginSayisi = 0,
                    SifreHash = _passwordHasher.HashPassword(null, dto.Sifre)
                };

                if (dto.IlgiAlaniIdleri != null && dto.IlgiAlaniIdleri.Any())
                {
                    var ilgiAlanlari = await _ilgiAlaniRepository.GetByIdsAsync(dto.IlgiAlaniIdleri);
                    yeniKullanici.IlgiAlanlari = ilgiAlanlari.ToList();
                }

                await _kullaniciRepository.AddAsync(yeniKullanici);

                var mesaj = yeniKullanici.Rol == "Admin" 
                    ? "Admin hesabı başarıyla oluşturuldu." 
                    : "Kayıt başarılı, onay bekleniyor.";

                return new KayitDto
                {
                    Basarili = true,
                    Mesaj = mesaj,
                    Id = yeniKullanici.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı kaydı sırasında bir hata oluştu.");
                return new KayitDto { Basarili = false, Mesaj = "Kayıt işlemi sırasında bir hata oluştu." };
            }
        }

        public async Task<GirisDto> GirisYapAsync(KullaniciGirisDto dto)
        {
            try
            {
                var kullanici = await _kullaniciRepository.GetByEmailAsync(dto.Email);
                if (kullanici == null)
                {
                    return new GirisDto { Basarili = false, Mesaj = "Kullanıcı bulunamadı." };
                }

                var dogrulamaSonucu = _passwordHasher.VerifyHashedPassword(kullanici, kullanici.SifreHash, dto.Sifre);
                if (dogrulamaSonucu == PasswordVerificationResult.Failed)
                {
                    return new GirisDto { Basarili = false, Mesaj = "Şifre yanlış." };
                }

                if (!kullanici.OnayliMi)
                {
                    return new GirisDto { Basarili = false, Mesaj = "Hesabınız henüz onaylanmamış." };
                }

                kullanici.LoginSayisi++;
                await _kullaniciRepository.UpdateAsync(kullanici);

                var token = TokenOlustur(kullanici);

                return new GirisDto
                {
                    Basarili = true,
                    Mesaj = "Giriş başarılı.",
                    Token = token,
                    SifreYenilemeGerekli = kullanici.LoginSayisi <= 1,
                    Rol = kullanici.Rol,
                    KullaniciId = kullanici.Id,
                    Email = kullanici.Email,
                    LoginSayisi = kullanici.LoginSayisi
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı girişi sırasında bir hata oluştu.");
                return new GirisDto { Basarili = false, Mesaj = "Giriş işlemi sırasında bir hata oluştu." };
            }
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

                kullanici.SifreHash = _passwordHasher.HashPassword(kullanici, dto.YeniSifre);
                await _kullaniciRepository.UpdateAsync(kullanici);

                _logger.LogInformation($"Şifre başarıyla değiştirildi. Kullanıcı ID: {kullanici.Id}");
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
