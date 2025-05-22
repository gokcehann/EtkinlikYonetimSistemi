using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IKullaniciService
    {
        Task<KayitDto> KayitOlAsync(KullaniciKayitDto dto);
        //kayitol metodu kullanıcıdan gelen dtoyu alır ve bir sonuç döndürür

        Task<GirisDto> GirisYapAsync(KullaniciGirisDto dto);

        Task<SifreDegistirSonucDto> SifreDegistirAsync(SifreDegistirDto dto);

    }
}
