namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class SifreDegistirDto
    {
        public int KullaniciId { get; set; }
        public required string Email { get; set; }
        public required string YeniSifre { get; set; }
    }
}
