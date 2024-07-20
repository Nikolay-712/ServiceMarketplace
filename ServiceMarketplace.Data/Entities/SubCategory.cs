namespace ServiceMarketplace.Data.Entities;

public class SubCategory : BaseEntity
{
    public Guid Id { get; set; }

    public string NameBg { get; set; }

    public string NameEn { get; set; }

    public string DescriptionBg { get; set; }

    public string DescriptionEn { get; set; }

    public Guid CategoryId { get; set; }

    public virtual ICollection<Tag> Tags { get; set; }

    public virtual ICollection<Service> Services { get; set; }
}
