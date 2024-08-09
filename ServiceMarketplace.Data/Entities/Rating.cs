namespace ServiceMarketplace.Data.Entities;

public class Rating : BaseEntity
{
    public Guid Id { get; set; }

    public int Value { get; set; }

    public string Comment { get; set; }

    public Guid ServiceId { get; set; }

    public Guid UserId { get; set; }

    public ApplicationUser User { get; set; }

    public int OwnerCommentId { get; set; }

    public OwnerComment OwnerComment { get; set; }
}
