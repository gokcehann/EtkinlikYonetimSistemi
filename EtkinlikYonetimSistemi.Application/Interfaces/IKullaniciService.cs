using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public interface IKullaniciService
    {
        Task<KayitDto> KayitOlAsync(KullaniciKayitDto dto);
        //kayitol metodu kullanıcıdan gelen dtoyu alır ve bir sonuç döndürür
    }
}
