using System.ComponentModel.DataAnnotations;

namespace EtkinlikYonetimSistemi.Web.Models
{
    public class KayitViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur.")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Sifre", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string SifreTekrar { get; set; }

        [Required(ErrorMessage = "İlgi alanı seçmek zorunludur.")]
        public List<int> IlgiAlaniIdleri { get; set; } = new();
    }
}