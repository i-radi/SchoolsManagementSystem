using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Models.Entities.Identity;

namespace SMS.Persistance.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
           .HasKey(x => x.Id);

        builder.HasOne(u => u.Organization)
                 .WithMany(d => d.Users)
                 .HasForeignKey(k => k.OrganizationId);
    }
}
