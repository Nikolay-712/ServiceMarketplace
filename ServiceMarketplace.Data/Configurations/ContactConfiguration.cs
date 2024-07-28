using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> contact)
    {
        contact
            .HasKey(x => x.Id);

        contact
            .Property(x => x.Name)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(150);

        contact
            .Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);
    }
}
