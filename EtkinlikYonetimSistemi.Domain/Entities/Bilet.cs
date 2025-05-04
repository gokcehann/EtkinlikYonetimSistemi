using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Bilet
    {
        public int BiletId { get; set; } 
        public int EtkinlikId { get; set; }        
        public Etkinlik Etkinlik { get; set; } // Navigation Property
        public decimal Fiyat { get; set; } // Bilet fiyatı
        public int StokAdedi { get; set; } // Kaç adet bilet kaldı?
        public ICollection<SepetBileti> SepetBiletleri { get; set; } // Sepet ile ilişki
    }
}
