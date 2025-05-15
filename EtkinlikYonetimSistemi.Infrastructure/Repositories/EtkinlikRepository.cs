using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikYonetimSistemi.Infrastructure.Repositories
{
    public class EtkinlikRepository : IEtkinlikRepository
    {
        private readonly EventDbContext _context;

        public EtkinlikRepository(EventDbContext context)
        {
            _context = context;
        }

        public async Task<List<Etkinlik>> GetAllAsync()
        {
            return await _context.Etkinlikler
                .Include(e => e.IlgiAlani)
                .Include(e => e.Biletler)
                .ToListAsync();
        }

        public async Task<Etkinlik> GetByIdAsync(int id)
        {
            return await _context.Etkinlikler
                .Include(e => e.IlgiAlani)
                .Include(e => e.Biletler)
                .FirstOrDefaultAsync(e => e.EtkinlikId == id);
        }

        public async Task<List<Etkinlik>> GetByKategoriAsync(int kategoriId)
        {
            return await _context.Etkinlikler
                .Include(e => e.IlgiAlani)
                .Include(e => e.Biletler)
                .Where(e => e.KategoriId == kategoriId)
                .ToListAsync();
        }

        public async Task AddAsync(Etkinlik etkinlik)
        {
            await _context.Etkinlikler.AddAsync(etkinlik);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Etkinlik etkinlik)
        {
            _context.Etkinlikler.Update(etkinlik);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var etkinlik = await GetByIdAsync(id);
            if (etkinlik != null)
            {
                _context.Etkinlikler.Remove(etkinlik);
                await _context.SaveChangesAsync();
            }
        }
    }
}