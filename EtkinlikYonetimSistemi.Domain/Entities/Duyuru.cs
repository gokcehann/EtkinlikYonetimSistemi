namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Duyuru
    {
        public int DuyuruId { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}