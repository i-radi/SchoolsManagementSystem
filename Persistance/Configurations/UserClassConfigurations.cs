using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;


public class UserClassConfigurations : IEntityTypeConfiguration<UserClass>
{
    public void Configure(EntityTypeBuilder<UserClass> builder)
    {
        builder
           .HasKey(x => x.Id);

        builder.HasOne(u => u.Season)
                 .WithMany(d => d.UserClasses)
                 .HasForeignKey(k => k.SeasonId)
                 .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(u => u.Classroom)
         .WithMany(d => d.UserClasses)
         .HasForeignKey(k => k.ClassroomId)
         .OnDelete(DeleteBehavior.NoAction);
    }
}

