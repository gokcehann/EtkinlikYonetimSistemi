namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Kullanici
    {
        public int Id { get; set; }
        public required string Ad { get; set; }
        public required string Soyad { get; set; }
        public required string Email { get; set; }
        public required string SifreHash { get; set; }
        public bool OnayliMi { get; set; }
        public int LoginSayisi { get; set; } // Kullanıcının giriş sayısını takip eder
        public required string Rol { get; set; } // admin , kullanici 

        // İlişkiler
        public ICollection<Sepet> Sepetler { get; set; }  //İleride kullanıcıların aldıkları tüm biletleri/sepetleri görebilmek için.
        public ICollection<SatinAlim> SatinAlimlar { get; set; }
        public ICollection<IlgiAlani> IlgiAlanlari { get; set; } // Kullanıcının ilgi alanları
    }
}
