using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Data.Configurations.Entities
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(
            EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
                    new Hotel { Id = 1, Name = "Sun International", Address = "Cape Town", Rating = 4.5, CountryId = 2 },
                    new Hotel { Id = 2, Name = "Sandals Resort and Spa", Address = "Negril", Rating = 1.2, CountryId = 1 },
                    new Hotel { Id = 3, Name = "Opera House", Address = "Sydney", Rating = 4.9, CountryId = 3 }
                );
        }
    }
}
