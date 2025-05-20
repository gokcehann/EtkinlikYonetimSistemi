namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class EtkinlikDto
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
        public string Sehir { get; set; }
        public bool PlanlanabilirMi { get; set; }
        public string PlanlamaMesaji { get; set; }
    }
}