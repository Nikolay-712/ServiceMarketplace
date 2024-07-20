using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> appUser)
    {
        appUser
            .HasMany(e => e.Claims)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        appUser
            .HasMany(e => e.Logins)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        appUser
            .HasMany(e => e.Roles)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        appUser
            .HasMany(x => x.Services)
            .WithOne(x => x.Owner)
            .HasForeignKey(x => x.OwnerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
