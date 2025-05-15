namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class SifreDegistirDto
    {
        public string Email { get; set; } = string.Empty;
        public string YeniSifre { get; set; } = string.Empty;
    }

    public class SifreDegistirSonucDto
    {
        public bool Basarili { get; set; }
        public string Mesaj { get; set; } = string.Empty;
    }
}
