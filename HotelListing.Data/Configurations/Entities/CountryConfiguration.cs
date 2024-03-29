﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Data.Configurations.Entities
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(
            EntityTypeBuilder<Country> builder)
        {
            builder
                .HasData(
                    new Country { Id = 1, Name = "Jamaica", ShortName = "JM" },
                    new Country { Id = 2, Name = "South Africa", ShortName = "ZA" },
                    new Country { Id = 3, Name = "Australia", ShortName = "AU" }
                );
        }
    }
}
