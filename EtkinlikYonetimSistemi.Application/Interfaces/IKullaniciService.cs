using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Services
{
    public interface IKullaniciService
    {
        KayitDto KayitOl(KullaniciKayitDto dto);
        //kayitol metodu kullanıcıdan gelen dtoyu alır ve bir sonuç döndürür
    }
}
