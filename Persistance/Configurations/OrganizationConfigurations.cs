using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class OrganizationConfigurations : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasData(
            new List<Organization>
            {
                new Organization
                {
                    Id = 1,
                    Name = "Cairo Organization"
                },
            });
    }
}
