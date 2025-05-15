using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IEtkinlikService
    {
        Task<List<EtkinlikSonucDto>> GetAllAsync();
        Task<EtkinlikSonucDto> GetByIdAsync(int id);
        Task<List<EtkinlikSonucDto>> GetByKategoriAsync(int kategoriId);
        Task<EtkinlikSonucDto> CreateAsync(EtkinlikOlusturDto dto);
        Task<EtkinlikSonucDto> UpdateAsync(EtkinlikGuncelleDto dto);
        Task<EtkinlikSonucDto> DeleteAsync(int id);
    }
}