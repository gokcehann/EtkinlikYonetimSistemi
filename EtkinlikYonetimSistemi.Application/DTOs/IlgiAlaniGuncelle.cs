namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class IlgiAlaniGuncelle
    {
        public int Id { get; set; }
        public List<int> IlgiAlaniIdleri { get; set; } = new();
    }
}
