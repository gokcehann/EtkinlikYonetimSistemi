using EtkinlikYonetimSistemi.Domain.Entities;
using EtkinlikYonetimSistemi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EtkinlikYonetimSistemi.Infrastructure.Context
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions<EventDbContext> options) : base(options)
        {
        }
        //constructor metod bu.optionun amacı veri bağlantı ayarlarını çekmek. base'in amacı dbcontexti bağlamak.

        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Etkinlik> Etkinlikler { get; set; }
        public DbSet<Bilet> Biletler { get; set; }
        public DbSet<Sepet> Sepetler { get; set; }
        public DbSet<SepetBileti> SepetBiletleri { get; set; }
        public DbSet<SatinAlim> SatinAlimlar { get; set; }
        public DbSet<SatinAlimBileti> SatinAlimBiletleri { get; set; }
        public DbSet<IlgiAlani> IlgiAlanlari { get; set; }
        //dbsetlerin amacı veri tabanındaki tabloları temsil etmek.

        //amacı ilişkileri ve özelleştirmeleri yapar.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Composite Key'ler
            modelBuilder.Entity<SepetBileti>()
                .HasKey(sb => new { sb.SepetId, sb.BiletId });

            modelBuilder.Entity<SatinAlimBileti>()
                .HasKey(sa => new { sa.SatinAlimId, sa.BiletId });

            // İlişkiler
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

            // Kullanici ve IlgiAlani arasındaki many-to-many ilişki
            modelBuilder.Entity<Kullanici>()
                .HasMany(k => k.IlgiAlanlari)
                .WithMany(i => i.Kullanicilar)
                .UsingEntity(j => j.ToTable("IlgiAlaniKullanici"));

            // Seed Data
            modelBuilder.Entity<IlgiAlani>().HasData(SeedData.GetIlgiAlanlari());
        }
    }
}
