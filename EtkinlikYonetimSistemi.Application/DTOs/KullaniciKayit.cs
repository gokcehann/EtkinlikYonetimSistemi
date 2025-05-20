using System.ComponentModel.DataAnnotations;

namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class KullaniciKayitDto
    {
        [Required(ErrorMessage = "Ad zorunludur.")]
        public required string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        public required string Soyad { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public required string Sifre { get; set; }

        [Required(ErrorMessage = "İlgi alanı seçmek zorunludur.")]
        public required List<int> IlgiAlaniIdleri { get; set; }
        public string Rol { get; set; }
        public bool OnayliMi { get; set; }
    }

    public class KayitDto
    {
        public required bool Basarili { get; set; }
        public required string Mesaj { get; set; }
        public int Id { get; set; }
    }
}
