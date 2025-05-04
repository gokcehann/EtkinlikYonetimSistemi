using EtkinlikYonetimSistemi.Application.Interfaces;
using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikYonetimSistemi.Infrastructure.Repositories
{
    /// Kullanıcı verilerinin veritabanı işlemlerini gerçekleştiren repository sınıfı.
    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly EventDbContext _context;
        //Veritabanı bağlantısı için DbContext nesnesi
        public KullaniciRepository(EventDbContext context)
        {
            _context = context;
        }

        // Belirtilen ID'ye sahip kullanıcıyı ve ilişkili ilgi alanlarını getirir.
        public async Task<Kullanici?> GetByIdAsync(int id)
        {
            return await _context.Kullanicilar
                .Include(k => k.IlgiAlanlari)
                .FirstOrDefaultAsync(k => k.Id == id);

        }

        /// Belirtilen email adresine sahip kullanıcıyı ve ilişkili ilgi alanlarını getirir.
        public async Task<Kullanici?> GetByEmailAsync(string email)
        {
            return await _context.Kullanicilar
                .Include(k => k.IlgiAlanlari)
                .FirstOrDefaultAsync(k => k.Email == email);
        }

        // Tüm kullanıcıları ve ilişkili ilgi alanlarını listeler.
        public async Task<List<Kullanici>> GetAllAsync()
        {
            return await _context.Kullanicilar
                .Include(k => k.IlgiAlanlari)
                .ToListAsync();
        }

        /// Yeni bir kullanıcı ekler ve değişiklikleri kaydeder.
        public async Task AddAsync(Kullanici kullanici)
        {
            await _context.Kullanicilar.AddAsync(kullanici);
            await _context.SaveChangesAsync();
        }

        /// Mevcut bir kullanıcıyı günceller ve değişiklikleri kaydeder.
        public async Task UpdateAsync(Kullanici kullanici)
        {
            _context.Kullanicilar.Update(kullanici);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı siler ve değişiklikleri kaydeder.
        /// Kullanıcı bulunamazsa işlem yapılmaz.
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
        /// Belirtilen email adresine sahip bir kullanıcının var olup olmadığını kontrol eder.
        /// </summary>
        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Kullanicilar.AnyAsync(k => k.Email == email);
        }
    }
}