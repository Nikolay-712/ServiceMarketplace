namespace ServiceMarketplace.Data.Entities;

public class Service : BaseEntity
{
    public Guid Id { get; set; }

    public string NameBg { get; set; }

    public string NameEn { get; set; }

    public string DescriptionBg { get; set; }

    public string DescriptionEn { get; set; }

    public Guid SubCategoryId { get; set; }

    public SubCategory SubCategory { get; set; }

    public Guid OwnerId { get; set; }

    public ApplicationUser Owner { get; set; }

    public int OfferedAtId { get; set; }

    public OfferedAt OfferedAt { get; set; }

    public virtual ICollection<ServiceCity> Cities { get; set; }

    public virtual ICollection<ServiceTag> SelectedTags { get; set; }

    public virtual ICollection<Contact> Contacts { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; }
}
