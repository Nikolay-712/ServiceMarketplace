﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data;

public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public ApplicationContext(DbContextOptions options)
       : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<SubCategory> SubCategories { get; set; }

    public DbSet<Tag> Tags { get; set; }

    public DbSet<City> Cities { get; set; }

    public DbSet<Service> Services { get; set; }

    public DbSet<ServiceCity> ServiceCities { get; set; }

    public DbSet<ServiceTag> ServiceTags { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<OfferedAt> OfferedAt { get; set; }

    public DbSet<Rating> Ratings { get; set; }

    public DbSet<OwnerComment> OwnerComments { get; set; }

    public DbSet<BusinessHours> BusinessHours { get; set; }

    public DbSet<ServiceCost> ServiceCosts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        this.ConfigureEntitiesRelations(builder);
    }

    private void ConfigureEntitiesRelations(ModelBuilder builder)
            => builder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
}
