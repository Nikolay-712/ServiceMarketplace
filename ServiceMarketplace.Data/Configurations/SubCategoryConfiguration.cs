using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> subCategory)
    {
        subCategory
            .HasKey(x => x.Id);

        subCategory
            .Property(x => x.NameBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(100);

        subCategory
            .Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(100);

        subCategory
            .Property(x => x.DescriptionBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(500);

        subCategory
            .Property(x => x.DescriptionEn)
            .IsRequired()
            .HasMaxLength(500);

        subCategory
            .HasMany(x => x.Tags)
            .WithOne()
            .HasForeignKey(x => x.SubCategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
