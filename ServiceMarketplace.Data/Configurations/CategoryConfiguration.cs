using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> category)
    {
        category
            .HasKey(x => x.Id);

        category
            .Property(x => x.NameBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(100);

        category
            .Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(100);

        category
            .Property(x => x.DescriptionBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(500);

        category
            .Property(x => x.DescriptionEn)
            .IsRequired()
            .HasMaxLength(500);

        category
            .HasMany(x => x.SubCategories)
            .WithOne()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
