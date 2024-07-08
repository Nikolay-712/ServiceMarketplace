using Microsoft.AspNetCore.Identity;

namespace ServiceMarketplace.Data.Entities;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public DateTime CreatedOn { get; set; }

    public string DescriptionEn { get; set; }

    public string DescriptionBg { get; set; }
}
