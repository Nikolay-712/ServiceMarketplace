namespace ServiceMarketplace.Data.Entities;

public class ServiceTag
{
    public Guid ServiceId { get; set; }

    public Service Service { get; set; }

    public int TagId { get; set; }

    public Tag Tag { get; set; }
}
