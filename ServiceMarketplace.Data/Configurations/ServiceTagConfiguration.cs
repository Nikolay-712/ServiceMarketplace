using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class ServiceTagConfiguration : IEntityTypeConfiguration<ServiceTag>
{
    public void Configure(EntityTypeBuilder<ServiceTag> serviceTag)
    {
        serviceTag
            .HasKey(x => new { x.ServiceId, x.TagId });

        serviceTag
            .HasOne(x => x.Service)
            .WithMany(x => x.SelectedTags)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        serviceTag
            .HasOne(x => x.Tag)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.TagId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
