using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

public class ServiceCostConfiguration : IEntityTypeConfiguration<ServiceCost>
{
    public void Configure(EntityTypeBuilder<ServiceCost> serviceCost)
    {
        serviceCost
            .HasKey(x => x.Id);

        serviceCost.Property(x => x.Price)
            .HasColumnType("decimal")
            .HasPrecision(10,2);

        serviceCost
            .Property(x => x.PricingType)
            .IsRequired();
    }
}
