using EtkinlikYonetimSistemi.Application.DTOs;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IDuyuruService
    {
        Task<List<DuyuruDto>> GetAllAsync();
        Task<DuyuruDto> GetByIdAsync(int id);
        Task<DuyuruDto> CreateAsync(DuyuruOlusturDto dto);
        Task<DuyuruDto> UpdateAsync(DuyuruGuncelleDto dto);
        Task<bool> DeleteAsync(int id);
    }
}