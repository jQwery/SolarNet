using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.EntitiesConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<DBCategory>
    {
        ///<inheritdoc />>
        public void Configure(EntityTypeBuilder<DBCategory> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CategoryName).IsRequired();
            builder.HasMany(x => x.Advertisments).WithOne(x => x.DBCategory).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.UnderCategories).WithOne(x => x.Parent).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId)
                .HasPrincipalKey(x => x.Id);
        }
    }
}