namespace ServiceMarketplace.Data.Entities;

public class Category : BaseEntity
{
    public Guid Id { get; set; }

    public string NameBg { get; set; }

    public string NameEn { get; set; }

    public string DescriptionBg { get; set; }

    public string DescriptionEn { get; set; }

    public virtual ICollection<SubCategory> SubCategories { get; set; }
}
