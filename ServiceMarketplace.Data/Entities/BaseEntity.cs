namespace ServiceMarketplace.Data.Entities;

public class BaseEntity
{
    protected BaseEntity()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
