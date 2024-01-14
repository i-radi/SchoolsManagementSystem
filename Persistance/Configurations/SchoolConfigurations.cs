using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class SchoolConfigurations : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasData(
            new List<School>
            {
                new School
                {
                    Id = 1,
                    Name = "Cairo 1 School",
                    Description = "desc. of Cairo 1 School",
                    OrganizationId = 1,
                }
            });
    }
}
