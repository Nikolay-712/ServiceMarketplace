namespace ServiceMarketplace.Data.Entities;

public class OfferedAt
{
    public int Id { get; set; }

    public string NameBg { get; set; }

    public string NameEn { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}
