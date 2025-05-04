using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Sepet
    {
        public int SepetId { get; set; } 
        public int KullaniciId { get; set; } 
        public Kullanici Kullanici { get; set; } // Navigation Property (ilişki)
        public DateTime OlusturmaTarihi { get; set; } // Sepetin oluşturulduğu zaman
        public ICollection<SepetBileti> SepetBiletleri { get; set; } // Sepetteki biletler
    }
}
