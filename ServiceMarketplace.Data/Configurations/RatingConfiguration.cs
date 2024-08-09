using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> rating)
    {
        rating
            .HasKey(x => x.Id);

        rating
            .Property(x => x.Value)
            .IsRequired();

        rating
            .Property(x => x.Comment)
            .HasMaxLength(600)
            .IsUnicode();

        rating
            .Property(x => x.CreatedOn)
            .IsRequired();

        rating
            .HasOne(x => x.User)
            .WithMany(x => x.Ratings)
            .IsRequired()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        rating
            .HasOne(x => x.OwnerComment)
            .WithOne(x => x.Rating)
            .HasForeignKey<OwnerComment>(x => x.RatingId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
