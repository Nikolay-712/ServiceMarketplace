namespace ServiceMarketplace.Data.Entities;

public class Contact
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string? LocationUrl { get; set; }

    public Guid ServiceId { get; set; }
}
