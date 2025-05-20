using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EtkinlikYonetimSistemi.Infrastructure.Context
{
    public class EventDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .AddDebug()
                    .SetMinimumLevel(LogLevel.Information);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Bilet> Biletler { get; set; }
        public DbSet<Sepet> Sepetler { get; set; }
        public DbSet<SepetBileti> SepetBiletleri { get; set; }
        public DbSet<SatinAlim> SatinAlimlar { get; set; }
        public DbSet<SatinAlimBileti> SatinAlimBiletleri { get; set; }
        public DbSet<IlgiAlani> IlgiAlanlari { get; set; }
        public DbSet<Duyuru> Duyurular { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SepetBileti>()
                .HasKey(sb => new { sb.SepetId, sb.BiletId });

            modelBuilder.Entity<SatinAlimBileti>()
                .HasKey(sa => new { sa.SatinAlimId, sa.BiletId });

            modelBuilder.Entity<Etkinlik>()
                .HasOne(e => e.IlgiAlani)
                .WithMany(i => i.Etkinlikler)
                .HasForeignKey(e => e.KategoriId);

            modelBuilder.Entity<Bilet>()
                .HasOne(b => b.Etkinlik)
                .WithMany(e => e.Biletler)
                .HasForeignKey(b => b.EtkinlikId);

            modelBuilder.Entity<SepetBileti>()
                .HasOne(sb => sb.Sepet)
                .WithMany(s => s.SepetBiletleri)
                .HasForeignKey(sb => sb.SepetId);

            modelBuilder.Entity<SepetBileti>()
                .HasOne(sb => sb.Bilet)
                .WithMany()
                .HasForeignKey(sb => sb.BiletId);

            modelBuilder.Entity<SatinAlim>()
                .HasOne(s => s.Kullanici)
                .WithMany()
                .HasForeignKey(s => s.Id);

            modelBuilder.Entity<SatinAlimBileti>()
                .HasOne(sa => sa.SatinAlim)
                .WithMany(s => s.SatinAlimBiletleri)
                .HasForeignKey(sa => sa.SatinAlimId);

            modelBuilder.Entity<SatinAlimBileti>()
                .HasOne(sa => sa.Bilet)
                .WithMany()
                .HasForeignKey(sa => sa.BiletId);

            modelBuilder.Entity<Kullanici>()
                .HasMany(k => k.IlgiAlanlari)
                .WithMany(i => i.Kullanicilar)
                .UsingEntity(j => j.ToTable("IlgiAlaniKullanici"));

            modelBuilder.Entity<IlgiAlani>().HasData(SeedData.GetIlgiAlanlari());
        }
    }
}
