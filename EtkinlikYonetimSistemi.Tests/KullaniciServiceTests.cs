using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Application.Services;
using EtkinlikYonetimSistemi.Domain.Entities;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace EtkinlikYonetimSistemi.Tests.Services
{
    public class KullaniciServiceTests
    {
        private readonly Mock<IKullaniciRepository> _mockKullaniciRepo = new();
        private readonly Mock<IIlgiAlaniRepository> _mockIlgiAlaniRepo = new();
        private readonly KullaniciService _service;

        public KullaniciServiceTests()
        {
            _service = new KullaniciService(_mockKullaniciRepo.Object, _mockIlgiAlaniRepo.Object);
        }

        [Fact]
        public async Task KayitOl_EmailZatenKayitliysa_BasarisizDonmeli()
        {
            // Arrange
            var dto = new KullaniciKayitDto
            {
                Ad = "Ali",
                Soyad = "Ylmaz",
                Email = "ali@example.com",
                Sifre = "sifre123",
                IlgiAlaniIdleri = new List<int> { 1, 2 }
            };

            _mockKullaniciRepo.Setup(r => r.GetByEmailAsync(dto.Email))
                              .ReturnsAsync(new Kullanici());

            // Act
            var result = await _service.KayitOlAsync(dto);

            // Assert
            Assert.False(result.Basarili);
            Assert.Equal("Bu email zaten kayıtlı.", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_EmailBos_Olmali()
        {
            // Arrange
            var dto = new KullaniciKayitDto
            {
                Ad = "Ali",
                Soyad = "Ylmaz",
                Email = "",
                Sifre = "sifre123",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            var result = await _service.KayitOlAsync(dto);

            // Assert
            Assert.False(result.Basarili);
            Assert.Contains("Email", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_SifreBos_Olmali()
        {
            var dto = new KullaniciKayitDto
            {
                Ad = "Ali",
                Soyad = "Ylmaz",
                Email = "ali@example.com",
                Sifre = "",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            var result = await _service.KayitOlAsync(dto);

            Assert.False(result.Basarili);
            Assert.Contains("Şifre", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_AdBos_Olmali()
        {
            var dto = new KullaniciKayitDto
            {
                Ad = "",
                Soyad = "Ylmaz",
                Email = "ali@example.com",
                Sifre = "123",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            var result = await _service.KayitOlAsync(dto);

            Assert.False(result.Basarili);
            Assert.Contains("Ad", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_SoyadBos_Olmali()
        {
            var dto = new KullaniciKayitDto
            {
                Ad = "Ali",
                Soyad = "",
                Email = "ali@example.com",
                Sifre = "123",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            var result = await _service.KayitOlAsync(dto);

            Assert.False(result.Basarili);
            Assert.Contains("Soyad", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_GecerliVerilerle_BasariliKayitOlmali()
        {
            var dto = new KullaniciKayitDto
            {
                Ad = "Ayse",
                Soyad = "Kara",
                Email = "ayse@example.com",
                Sifre = "gizli123",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            _mockKullaniciRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((Kullanici?)null);
            _mockIlgiAlaniRepo.Setup(r => r.GetByIdsAsync(dto.IlgiAlaniIdleri))
                              .ReturnsAsync(new List<IlgiAlani> { new IlgiAlani { Id = 1, Ad = "MÃ¼zik" } });

            _mockKullaniciRepo.Setup(r => r.AddAsync(It.IsAny<Kullanici>()))
                              .Returns(Task.CompletedTask);

            var result = await _service.KayitOlAsync(dto);

            Assert.True(result.Basarili);
            Assert.Equal("Kayıt başarılı, onay bekleniyor.", result.Mesaj);
        }

        [Fact]
        public async Task GirisYap_HataliEmail_Olmamali()
        {
            // Arrange
            var dto = new KullaniciGirisDto
            {
                Email = "yanlis@example.com",
                Sifre = "DogruSifre123"
            };

            _mockKullaniciRepo.Setup(r => r.GetByEmailAsync(dto.Email))
                              .ReturnsAsync((Kullanici?)null); // KullanÄ±cÄ± yok

            // Act
            var sonuc = await _service.GirisYapAsync(dto);

            // Assert
            Assert.False(sonuc.Basarili);
            Assert.Equal("Kullanıcı bulunamadı.", sonuc.Mesaj);
        }

        [Fact]
        public async Task GirisYap_HataliSifre_Olmamali()
        {
            // Arrange
            var dto = new KullaniciGirisDto
            {
                Email = "dogru@example.com",
                Sifre = "YanlisSifre123"
            };

            var passwordHasher = new PasswordHasher<Kullanici>();
            var kullanici = new Kullanici
            {
                Email = dto.Email,
                SifreHash = passwordHasher.HashPassword(null, "GercekSifre123"),
                LoginSayisi = 0
            };

            _mockKullaniciRepo.Setup(r => r.GetByEmailAsync(dto.Email))
                              .ReturnsAsync(kullanici);

            // Act
            var sonuc = await _service.GirisYapAsync(dto);

            // Assert
            Assert.False(sonuc.Basarili);
            Assert.Equal("Şifre yanlış.", sonuc.Mesaj);
        }

        [Fact]
        public async Task GirisYap_BosAlanlar_Olmamali()
        {
            // Arrange
            var dto = new KullaniciGirisDto
            {
                Email = "",
                Sifre = ""
            };

            // Act
            var sonuc = await _service.GirisYapAsync(dto);

            // Assert
            Assert.False(sonuc.Basarili);
            Assert.Equal("Email ve şifre boş olamaz.", sonuc.Mesaj);
        }


    }
}
