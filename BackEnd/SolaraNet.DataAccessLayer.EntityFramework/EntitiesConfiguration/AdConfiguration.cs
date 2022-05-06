using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.EntitiesConfiguration
{
    public class AdConfiguration : IEntityTypeConfiguration<DBAdvertisment>
    {
        public void Configure(EntityTypeBuilder<DBAdvertisment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AdvertismentTitle).IsRequired();
            builder.Property(x => x.City).IsRequired();
            builder.Property(x => x.Description).IsRequired();
            builder.Property(x => x.IsPayed).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.PublicationDate).IsRequired();
            builder.HasOne(x => x.DBCategory);
            builder.HasMany(x => x.Image).WithOne(x => x.Advertisment).OnDelete(DeleteBehavior.Cascade);
        }
    }
}