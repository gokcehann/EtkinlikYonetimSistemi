namespace EtkinlikYonetimSistemi.Application.DTOs
{
    public class KullaniciGirisDto
    {
        public string Email { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;

    }

    public class GirisDto
    {
        public bool Basarili { get; set; }
        public string Mesaj { get; set; } = string.Empty;
        public bool SifreYenilemeGerekli { get; set; }
        public object Token { get; internal set; }
    }
}
