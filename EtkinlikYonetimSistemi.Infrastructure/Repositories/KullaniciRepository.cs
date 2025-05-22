using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikYonetimSistemi.Infrastructure.Repositories
{
    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly EventDbContext _context;
        
        public KullaniciRepository(EventDbContext context)
        {
            _context = context;
        }

        // Belirtilen ID'ye sahip kullanÄ±cÄ±yÄ± ve iliÅŸkili ilgi alanlarÄ±nÄ± getirir.                
        public async Task<Kullanici?> GetByIdAsync(int id)
        {
            return await _context.Kullanicilar
                .Include(k => k.IlgiAlanlari)
                .FirstOrDefaultAsync(k => k.Id == id);
        }

        /// Belirtilen email adresine sahip kullanÄ±cÄ±yÄ± ve iliÅŸkili ilgi alanlarÄ±nÄ± getirir.
        public async Task<Kullanici?> GetByEmailAsync(string email)
        {
            return await _context.Kullanicilar
                .Include(k => k.IlgiAlanlari)
                .FirstOrDefaultAsync(k => k.Email == email);
        }

        // TÃ¼m kullanÄ±cÄ±larÄ± ve iliÅŸkili ilgi alanlarÄ±nÄ± listeler.
        public async Task<List<Kullanici>> GetAllAsync()
        {
            return await _context.Kullanicilar
                .Include(k => k.IlgiAlanlari)
                .ToListAsync();
        }

        /// Yeni bir kullanÄ±cÄ± ekler ve deÄŸiÅŸiklikleri kaydeder.
        public async Task AddAsync(Kullanici kullanici)
        {
            await _context.Kullanicilar.AddAsync(kullanici);
            await _context.SaveChangesAsync();
        }

        /// Mevcut bir kullanÄ±cÄ±yÄ± gÃ¼nceller ve deÄŸiÅŸiklikleri kaydeder.
        public async Task UpdateAsync(Kullanici kullanici)
        {
            _context.Kullanicilar.Update(kullanici);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanÄ±cÄ±yÄ± siler ve deÄŸiÅŸiklikleri kaydeder.
        /// KullanÄ±cÄ± bulunamazsa iÅŸlem yapÄ±lmaz.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var kullanici = await GetByIdAsync(id);
            if (kullanici != null)
            {
                _context.Kullanicilar.Remove(kullanici);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Belirtilen email adresine sahip bir kullanÄ±cÄ±nÄ±n var olup olmadÄ±ÄŸÄ±nÄ± kontrol eder.
        /// </summary>
        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Kullanicilar.AnyAsync(k => k.Email == email);
        }
    }
}
