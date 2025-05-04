using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Etkinlik
    {
        public int EtkinlikId { get; set; }
        public string Ad { get; set; }
        public string Aciklama { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public int Kapasite { get; set; }
        public int KalanBilet { get; set; }
        public int KategoriId { get; set; } // Kategori ID'si
        public bool AktifMi { get; set; }

        // Navigation properties
        public IlgiAlani IlgiAlani { get; set; } // Kategori ilişkisi
        public ICollection<Bilet> Biletler { get; set; } // Biletler ilişkisi
    }
}
