using EtkinlikYonetimSistemi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EtkinlikYonetimSistemi.Infrastructure
{
    public class EventDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
    {
        public EventDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventDbContext>();

            // Buraya senin connection string’ini koy:
            optionsBuilder.UseSqlServer("Server=GOKCEHAN;Database=EtkinlikYonetimDb;Trusted_Connection=True;TrustServerCertificate=True; MultipleActiveResultSets=true");

            return new EventDbContext(optionsBuilder.Options);
        }
    }
}
