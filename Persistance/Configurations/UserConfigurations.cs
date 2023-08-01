using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Identity;

namespace Persistance.Configurations;

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
