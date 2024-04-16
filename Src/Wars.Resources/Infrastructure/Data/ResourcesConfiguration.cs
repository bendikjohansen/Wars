using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wars.Resources.Infrastructure.Data;

internal class ResourcesConfiguration : IEntityTypeConfiguration<Domain.Resources>
{
    public void Configure(EntityTypeBuilder<Domain.Resources> builder)
    {
        builder.Property(r => r.Id).ValueGeneratedNever();
        builder.HasIndex(r => r.VillageId).IsUnique();
        builder.Property(r => r.VillageId).HasMaxLength(128);
    }
}
