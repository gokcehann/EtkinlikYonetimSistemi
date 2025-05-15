using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikYonetimSistemi.Infrastructure.Repositories
{
    public class IlgiAlaniRepository : IIlgiAlaniRepository
    {
        private readonly EventDbContext _context;

        public IlgiAlaniRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<IlgiAlani?> GetByIdAsync(int id)
        {
            return await _context.IlgiAlanlari
                .FirstOrDefaultAsync(i => i.IlgiAlaniId == id);
        }

        public async Task<List<IlgiAlani>> GetAllAsync()
        {
            return await _context.IlgiAlanlari.ToListAsync();
        }

        public async Task<List<IlgiAlani>> GetByIdsAsync(List<int> ids)
        {
            return await _context.IlgiAlanlari
                .Where(i => ids.Contains(i.IlgiAlaniId))
                .ToListAsync();

        }

        public async Task AddAsync(IlgiAlani ilgiAlani)
        {
            await _context.IlgiAlanlari.AddAsync(ilgiAlani);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IlgiAlani ilgiAlani)
        {
            _context.IlgiAlanlari.Update(ilgiAlani);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ilgiAlani = await GetByIdAsync(id);
            if (ilgiAlani != null)
            {
                _context.IlgiAlanlari.Remove(ilgiAlani);
                await _context.SaveChangesAsync();
            }
        }
    }
}
