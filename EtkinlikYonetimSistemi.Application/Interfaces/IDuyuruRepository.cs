using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IDuyuruRepository
    {
        Task<List<Duyuru>> GetAllAsync();
        Task<Duyuru> GetByIdAsync(int id);
        Task AddAsync(Duyuru duyuru);
        Task UpdateAsync(Duyuru duyuru);
        Task DeleteAsync(int id);
    }
}