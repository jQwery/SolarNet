using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.EntityFramework.EntitiesConfiguration
{
    public class CommentConfiguration : IEntityTypeConfiguration<DBComment>
    {
        public void Configure(EntityTypeBuilder<DBComment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CommentStatus).IsRequired();
            builder.Property(x => x.CommentText).IsRequired();
            builder.Property(x => x.PublicationDate).IsRequired();
            builder.HasOne(x => x.Advertisment).WithMany(x => x.Comments).HasForeignKey(x => x.AdvertismentId)
                .HasPrincipalKey(x => x.Id);
            builder.HasOne(x => x.User).WithMany(x=>x.Comments).HasForeignKey(x => x.UserId).HasPrincipalKey(x => x.Id);
        }
    }
}