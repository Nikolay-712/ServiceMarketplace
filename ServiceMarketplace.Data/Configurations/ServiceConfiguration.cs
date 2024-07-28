using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> service)
    {
        service
            .HasKey(x => x.Id);

        service
            .Property(x => x.NameBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(150);

        service
            .Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(150);

        service
            .Property(x => x.DescriptionBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(500);

        service
            .Property(x => x.DescriptionEn)
            .IsRequired()
            .HasMaxLength(500);

        service
            .HasOne(x => x.SubCategory)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.SubCategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        service
            .HasMany(x => x.Contacts)
            .WithOne()
            .HasForeignKey(x => x.ServiceId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
