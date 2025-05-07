using EtkinlikYonetimSistemi.Domain.Entities;

namespace EtkinlikYonetimSistemi.Application.Interfaces
{
    public interface IKullaniciRepository
    {
        Task<Kullanici?> GetByIdAsync(int id);
        Task<Kullanici?> GetByEmailAsync(string email);
        Task<List<Kullanici>> GetAllAsync();
        Task AddAsync(Kullanici kullanici);
        Task UpdateAsync(Kullanici kullanici);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string email);
    }
}
