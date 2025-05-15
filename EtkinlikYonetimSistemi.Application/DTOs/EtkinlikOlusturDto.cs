using System.ComponentModel.DataAnnotations;

namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class EtkinlikOlusturDto
    {
        [Required(ErrorMessage = "Etkinlik adı zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Etkinlik açıklaması zorunludur")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Başlangıç tarihi zorunludur")]
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

    public class EtkinlikSonucDto
    {
        public int EtkinlikId { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public int Kapasite { get; set; }
        public int KalanBilet { get; set; }
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public decimal BiletFiyati { get; set; }
        public HavaDurumuDto HavaDurumu { get; set; }
        public bool Basarili { get; set; }
        public string Mesaj { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}