namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class SatinAlim
    {
        public int SatinAlimId { get; set; }
        public int Id { get; set; }
        public DateTime SatinAlimTarihi { get; set; }

        // Navigation properties
        public Kullanici Kullanici { get; set; }
    }
}
