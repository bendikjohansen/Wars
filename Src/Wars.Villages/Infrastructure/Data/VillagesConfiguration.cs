using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wars.Villages.Domain;

namespace Wars.Villages.Infrastructure.Data;

internal class VillagesConfiguration : IEntityTypeConfiguration<Village>
{
    public void Configure(EntityTypeBuilder<Village> builder)
    {
        builder.Property(v => v.Id).ValueGeneratedNever();
        builder.Property(v => v.Name).HasMaxLength(128);
        builder.Property(v => v.OwnerId).HasMaxLength(128);
    }
}
