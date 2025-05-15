namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Etkinlik
    {
        public int EtkinlikId { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public int Kapasite { get; set; }
        public int KalanBilet { get; set; }
        public int KategoriId { get; set; }
        public decimal BiletFiyati { get; set; }

        // Navigation properties  
        public IlgiAlani IlgiAlani { get; set; }
        public ICollection<Bilet> Biletler { get; set; }
    }
}
