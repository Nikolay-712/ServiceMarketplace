using ServiceMarketplace.Data.Enums;

namespace ServiceMarketplace.Data.Entities;

public class ServiceCost
{
    public int Id { get; set; }

    public decimal? Price { get; set; }

    public PricingType PricingType { get; set; }

    public Guid ServiceId { get; set; }
}
