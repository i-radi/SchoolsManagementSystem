using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Identity;

namespace Persistance.Configurations;

public class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new List<Role>
            {
                new Role
                {
                    Id = 1,
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN"
                },
                new Role
                {
                    Id = 2,
                    Name = "OrganizationAdmin",
                    NormalizedName = "ORGANIZATIONADMIN"
                },
                new Role
                {
                    Id = 3,
                    Name = "SchoolAdmin",
                    NormalizedName = "SCHOOLADMIN"
                },
                new Role
                {
                    Id = 4,
                    Name = "FullAccessActivity",
                    NormalizedName = "FULLACCESSACTIVITY"
                },
                new Role
                {
                    Id = 5,
                    Name = "ReadAccessActivity",
                    NormalizedName = "READACCESSACTIVITY"
                }
            });
    }
}
