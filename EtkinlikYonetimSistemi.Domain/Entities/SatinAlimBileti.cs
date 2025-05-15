namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class SatinAlimBileti
    {
        public int SatinAlimId { get; set; }
        public int BiletId { get; set; }

        // Navigation properties
        public SatinAlim SatinAlim { get; set; }
        public Bilet Bilet { get; set; }
    }
}
