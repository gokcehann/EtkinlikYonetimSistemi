using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IHavaDurumuService
    {
        Task<HavaDurumuDto> GetHavaDurumuAsync(DateTime tarih, string sehir);
    }
}