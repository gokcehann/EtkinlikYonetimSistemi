using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikYonetimSistemi.Infrastructure.Repositories
{
    public class DuyuruRepository : IDuyuruRepository
    {
        private readonly EventDbContext _context;
        public DuyuruRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<List<Duyuru>> GetAllAsync()
        {
            return await _context.Set<Duyuru>().OrderByDescending(d => d.OlusturmaTarihi).ToListAsync();
        }

        public async Task<Duyuru> GetByIdAsync(int id)
        {
            return await _context.Set<Duyuru>().FirstOrDefaultAsync(d => d.DuyuruId == id);
        }

        public async Task AddAsync(Duyuru duyuru)
        {
            await _context.Set<Duyuru>().AddAsync(duyuru);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Duyuru duyuru)
        {
            _context.Set<Duyuru>().Update(duyuru);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var duyuru = await GetByIdAsync(id);
            if (duyuru != null)
            {
                _context.Set<Duyuru>().Remove(duyuru);
                await _context.SaveChangesAsync();
            }
        }
    }
}