
using System.ComponentModel.DataAnnotations;

namespace EtkinlikYonetimSistemi.Web.Models
{
    public class GirisViewModel
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz lütfen.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
}