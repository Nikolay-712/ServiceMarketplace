using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> tag)
    {
        tag
           .HasKey(x => x.Id);

        tag
            .Property(x => x.NameBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(100);

        tag
            .Property(x => x.NameEn)
            .IsRequired()
            .HasMaxLength(100);
    }
}
