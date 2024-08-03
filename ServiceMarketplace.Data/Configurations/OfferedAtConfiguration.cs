using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceMarketplace.Data.Entities;

namespace ServiceMarketplace.Data.Configurations;

internal class OfferedAtConfiguration : IEntityTypeConfiguration<OfferedAt>
{
    public void Configure(EntityTypeBuilder<OfferedAt> offered)
    {
        offered
            .HasKey(x => x.Id);

        offered
            .Property(x => x.NameBg)
            .IsRequired()
            .IsUnicode()
            .HasMaxLength(100);

        offered
           .Property(x => x.NameEn)
           .IsRequired()
           .HasMaxLength(100);
    }
}
