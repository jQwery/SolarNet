using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<DBUser>
    {
        public void Configure(EntityTypeBuilder<DBUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.UserName).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.HasOne(x => x.Avatar).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
        }
    }
}