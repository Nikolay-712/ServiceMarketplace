using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class ServiceCityConfiguration : IEntityTypeConfiguration<ServiceCity>
{
    public void Configure(EntityTypeBuilder<ServiceCity> serviceCity)
    {
        serviceCity
            .HasKey(x => new { x.ServiceId, x.CityId });

        serviceCity
            .HasOne(x => x.Service)
            .WithMany(x => x.Cities)
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        serviceCity
            .HasOne(x => x.City)
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
