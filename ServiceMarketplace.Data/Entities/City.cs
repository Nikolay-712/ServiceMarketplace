namespace ServiceMarketplace.Data.Entities;

public class City
{
    public Guid Id { get; set; }

    public string NameBg { get; set; }

    public string NameEn { get; set; }

    public virtual ICollection<ServiceCity> Services { get; set; }
}
