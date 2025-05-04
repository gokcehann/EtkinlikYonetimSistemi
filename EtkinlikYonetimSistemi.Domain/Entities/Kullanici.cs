using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class Kullanici
    {
            public int Id { get; set; }
            public string Ad { get; set; }
            public string Soyad { get; set; }
            public string Email { get; set; }
            public string SifreHash { get; set; }
            public bool OnayliMi { get; set; }
            public int LoginSayisi { get; set; } // Kullanıcının giriş sayısını takip eder
            public bool Rol { get; set; } // admin true, kullanici false

        // İlişkiler
        public ICollection<Sepet> Sepetler { get; set; }  //İleride kullanıcıların aldıkları tüm biletleri/sepetleri görebilmek için.
        public ICollection<SatinAlim> SatinAlimlar { get; set; }
        public ICollection<IlgiAlani> IlgiAlanlari { get; set; } // Kullanıcının ilgi alanları
    }
}
