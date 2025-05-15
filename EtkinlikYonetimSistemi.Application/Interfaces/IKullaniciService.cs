using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IKullaniciService
    {
        Task<KayitDto> KayitOlAsync(KullaniciKayitDto dto);
        //kayitol metodu kullanıcıdan gelen dtoyu alır ve bir sonuç döndürür

        Task<GirisDto> GirisYapAsync(KullaniciGirisDto dto);

        Task<KayitDto> SifreDegistirAsync(SifreDegistirDto dto);

        Task<bool> IlgiAlaniGuncelleAsync(IlgiAlaniGuncelle dto);


    }
}
