namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class DuyuruDto
    {
        public int DuyuruId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }

    public class DuyuruOlusturDto
    {
        public string Baslik { get; set; }
        public string Icerik { get; set; }
    }

    public class DuyuruGuncelleDto
    {
        public int DuyuruId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
    }
} 