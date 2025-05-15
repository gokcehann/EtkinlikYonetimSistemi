namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class HavaDurumuDto
    {
        public string Durum { get; set; } // Örn: "Güneşli", "Yağmurlu"
        public double Sicaklik { get; set; } // Celsius cinsinden
        public string Icon { get; set; } // Hava durumu ikonu
    }
}