using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wars.Resources.Domain;

namespace Wars.Resources.Infrastructure.Data;

internal class VillageConfiguration : IEntityTypeConfiguration<Village>
{
    public void Configure(EntityTypeBuilder<Village> builder)
    {
        builder.Property(v => v.Id).HasMaxLength(128);
        builder.ComplexProperty(v => v.ResourceInventory);
        builder.ComplexProperty(v => v.ResourceBuilding);
    }
}
