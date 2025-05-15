using System.ComponentModel.DataAnnotations;

namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class EtkinlikGuncelleDto
    {
        public int EtkinlikId { get; set; }

        [Required(ErrorMessage = "Etkinlik adı zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Etkinlik açıklaması zorunludur")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur")]
        public DateTime Tarih { get; set; }

        [Required(ErrorMessage = "Kapasite zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Kapasite en az 1 olmalıdır")]
        public int Kapasite { get; set; }

        [Required(ErrorMessage = "Kategori ID zorunludur")]
        public int KategoriId { get; set; }

        [Required(ErrorMessage = "Bilet fiyatı zorunludur")]
        [Range(0, double.MaxValue, ErrorMessage = "Bilet fiyatı 0'dan büyük olmalıdır")]
        public decimal BiletFiyati { get; set; }
    }
}