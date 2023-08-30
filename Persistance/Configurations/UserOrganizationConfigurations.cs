using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Identity;

namespace Persistance.Configurations;


public class UserOrganizationConfigurations : IEntityTypeConfiguration<UserOrganization>
{
    public void Configure(EntityTypeBuilder<UserOrganization> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .HasOne(u => u.User)
            .WithMany(ur => ur.UserOrganizations)
            .HasForeignKey(u => u.UserId);

        builder
            .HasOne(u => u.Organization)
            .WithMany(ur => ur.UserOrganizations)
            .HasForeignKey(u => u.OrganizationId);

    }
}

