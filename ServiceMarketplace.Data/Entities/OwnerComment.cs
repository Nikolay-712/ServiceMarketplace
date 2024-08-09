namespace ServiceMarketplace.Data.Entities;

public class OwnerComment : BaseEntity
{
    public int Id { get; set; }

    public string Comment { get; set; }

    public Guid RatingId { get; set; }

    public Rating Rating { get; set; }

    public Guid OwnerId { get; set; }
}
