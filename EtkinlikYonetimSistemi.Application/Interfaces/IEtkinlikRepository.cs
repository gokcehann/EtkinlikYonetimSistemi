using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IEtkinlikRepository
    {
        Task<List<Etkinlik>> GetAllAsync();
        Task<Etkinlik> GetByIdAsync(int id);
        Task<List<Etkinlik>> GetByKategoriAsync(int kategoriId);
        Task AddAsync(Etkinlik etkinlik);
        Task UpdateAsync(Etkinlik etkinlik);
        Task DeleteAsync(int id);
    }
}