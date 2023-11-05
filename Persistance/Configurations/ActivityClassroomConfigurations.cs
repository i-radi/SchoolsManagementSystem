using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class ActivityClassroomConfigurations : IEntityTypeConfiguration<ActivityClassroom>
{
    public void Configure(EntityTypeBuilder<ActivityClassroom> builder)
    {
        builder.HasOne(a => a.Classroom)
        .WithMany(a => a.ActivityClassrooms)
        .HasForeignKey(a => a.ClassroomId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
