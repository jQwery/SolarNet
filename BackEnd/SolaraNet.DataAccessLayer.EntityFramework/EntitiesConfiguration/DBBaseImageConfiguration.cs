using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.EntitiesConfiguration
{
    public class DBBaseImageConfiguration : IEntityTypeConfiguration<DBBaseImage>
    {
        public void Configure(EntityTypeBuilder<DBBaseImage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ImageLink).IsRequired();
        }
    }
}