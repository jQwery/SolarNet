using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.EntitiesConfiguration
{
    public class DBAvatarConfiguration : IEntityTypeConfiguration<DBAvatar>
    {
        public void Configure(EntityTypeBuilder<DBAvatar> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ImageLink).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasOne(x => x.User).WithOne(x => x.Avatar).OnDelete(DeleteBehavior.Cascade);
        }
    }
}