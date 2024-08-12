using Microsoft.AspNetCore.Identity;

namespace ServiceMarketplace.Data.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public string FullName { get; set; }

    public DateTime CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> Roles { get; set; }

    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }

    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }

    public virtual ICollection<Service> Services { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; }

    public virtual ICollection<OwnerComment> OwnerComments { get; set; }
}
