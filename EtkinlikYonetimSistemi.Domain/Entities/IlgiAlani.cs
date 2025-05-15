namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class IlgiAlani
    {
        public int IlgiAlaniId { get; set; } // Örnek: "1" (ana kategori), "1a" (alt kategori)
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public bool AnaKategoriMi { get; set; } // Bu kategori ana kategori mi?
        public int? UstKategoriId { get; set; }

        // Navigation Properties
        public IlgiAlani UstKategori { get; set; }
        public ICollection<IlgiAlani>? AltKategoriler { get; set; }
        public ICollection<Kullanici> Kullanicilar { get; set; }
        public ICollection<Etkinlik>? Etkinlikler { get; set; }

        public decimal BasePrice { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
