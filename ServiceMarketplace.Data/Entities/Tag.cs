namespace ServiceMarketplace.Data.Entities;

public class Tag
{
    public int Id { get; set; }

    public string NameBg { get; set; }

    public string NameEn { get; set; }

    public Guid SubCategoryId { get; set; }
}
