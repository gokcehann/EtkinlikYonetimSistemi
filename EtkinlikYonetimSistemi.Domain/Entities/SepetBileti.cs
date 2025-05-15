namespace EtkinlikYonetimSistemi.Domain.Entities
{
    public class SepetBileti
    {
        public int SepetId { get; set; } // Hangi sepete ait? (Foreign Key)
        public Sepet Sepet { get; set; } // Sepet nesnesi ile ilişki
        public int BiletId { get; set; } // Hangi bilete ait? (Foreign Key)
        public Bilet? Bilet { get; set; } // Bilet nesnesi ile ilişki
        public int Adet { get; set; } // Bu biletten sepette kaç adet var?
    }
}
