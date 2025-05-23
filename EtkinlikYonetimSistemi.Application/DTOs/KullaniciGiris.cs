﻿using System.ComponentModel.DataAnnotations;

namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class KullaniciGirisDto
    {
        public string Email { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }

    public class GirisDto
    {
        public bool Basarili { get; set; }
        public string Mesaj { get; set; } = string.Empty;
        public bool SifreYenilemeGerekli { get; set; }
        public string Token { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int KullaniciId { get; set; }
        public string Email { get; set; } = string.Empty;
        public int LoginSayisi { get; set; }
    }
}
