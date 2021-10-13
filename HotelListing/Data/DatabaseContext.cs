using Microsoft.EntityFrameworkCore;

namespace HotelListing.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasData(
                    new Country { Id = 1, Name = "Jamaica", ShortName = "JM" },
                    new Country { Id = 2, Name = "South Africa", ShortName = "ZA" },
                    new Country { Id = 3, Name = "Australia", ShortName = "AU" }
                );

            modelBuilder.Entity<Hotel>()
                .HasData(
                    new Hotel { Id = 1, Name = "Sun International", Address = "Cape Town", Rating = 4.5, CountryId = 2 },
                    new Hotel { Id = 2, Name = "Sandals Resort and Spa", Address = "Negril", Rating = 1.2, CountryId = 1 },
                    new Hotel { Id = 3, Name = "Opera House", Address = "Sydney", Rating = 4.9, CountryId = 3 }
                );
        }
    }
}