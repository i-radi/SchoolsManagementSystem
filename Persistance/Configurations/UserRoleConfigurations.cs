using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Identity;

namespace Persistance.Configurations;


public class UserRoleConfigurations : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .HasOne(u => u.Role)
            .WithMany(ur => ur.UserRoles)
            .HasForeignKey(u => u.RoleId);

        builder
            .HasOne(u => u.User)
            .WithMany(ur => ur.UserRoles)
            .HasForeignKey(u => u.UserId);

        builder.HasData(
            new List<UserRole>
            {
                new UserRole
                {
                    Id = 1,
                    UserId = 1,
                    RoleId = 1
                }
            });
    }
}

