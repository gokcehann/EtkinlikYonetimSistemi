using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IIlgiAlaniRepository
    {
        Task<IlgiAlani?> GetByIdAsync(int id);
        Task<List<IlgiAlani>> GetAllAsync();
        Task<List<IlgiAlani>> GetByIdsAsync(List<int> ids);
        Task AddAsync(IlgiAlani ilgiAlani);
        Task UpdateAsync(IlgiAlani ilgiAlani);
        Task DeleteAsync(int id);
    }
}
