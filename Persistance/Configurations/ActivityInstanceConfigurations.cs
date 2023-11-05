using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class ActivityInstanceConfigurations : IEntityTypeConfiguration<ActivityInstance>
{
    public void Configure(EntityTypeBuilder<ActivityInstance> builder)
    {
        builder.HasOne(a => a.Season)
        .WithMany(a => a.ActivityInstances)
        .HasForeignKey(a => a.SeasonId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
