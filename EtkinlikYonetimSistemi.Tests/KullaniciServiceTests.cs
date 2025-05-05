using EtkinlikYonetimSistemi.Application.DTOs;
using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Application.Services;
using EtkinlikYonetimSistemi.Domain.Entities;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                Soyad = "Yýlmaz",
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
            Assert.Equal("Bu email zaten kayýtlý.", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_EmailBos_Olmali()
        {
            // Arrange
            var dto = new KullaniciKayitDto
            {
                Ad = "Ali",
                Soyad = "Yýlmaz",
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
                Soyad = "Yýlmaz",
                Email = "ali@example.com",
                Sifre = "",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            var result = await _service.KayitOlAsync(dto);

            Assert.False(result.Basarili);
            Assert.Contains("Þifre", result.Mesaj);
        }

        [Fact]
        public async Task KayitOl_AdBos_Olmali()
        {
            var dto = new KullaniciKayitDto
            {
                Ad = "",
                Soyad = "Yýlmaz",
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
                Ad = "Ayþe",
                Soyad = "Kara",
                Email = "ayse@example.com",
                Sifre = "gizli123",
                IlgiAlaniIdleri = new List<int> { 1 }
            };

            _mockKullaniciRepo.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((Kullanici?)null);
            _mockIlgiAlaniRepo.Setup(r => r.GetByIdsAsync(dto.IlgiAlaniIdleri))
                              .ReturnsAsync(new List<IlgiAlani> { new IlgiAlani { Id = 1, Ad = "Müzik" } });

            _mockKullaniciRepo.Setup(r => r.AddAsync(It.IsAny<Kullanici>()))
                              .Returns(Task.CompletedTask);

            var result = await _service.KayitOlAsync(dto);

            Assert.True(result.Basarili);
            Assert.Equal("Kayýt baþarýlý, onay bekleniyor.", result.Mesaj);
        }
    }
}
