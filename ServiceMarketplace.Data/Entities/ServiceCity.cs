namespace ServiceMarketplace.Data.Entities;

public class ServiceCity
{
    public Guid ServiceId { get; set; }

    public Service Service { get; set; }

    public Guid CityId { get; set; }

    public City City { get; set; }
}
