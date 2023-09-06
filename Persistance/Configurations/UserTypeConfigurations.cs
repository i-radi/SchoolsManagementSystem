using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistance.Configurations;

public class UserTypeConfigurations : IEntityTypeConfiguration<UserType>
{
    public void Configure(EntityTypeBuilder<UserType> builder)
    {
        builder.HasData(
            new List<UserType>
            {
                new UserType{ Id = 1, Name = "Teacher"},
                new UserType{ Id = 2, Name = "Student"},
                new UserType{ Id = 3, Name = "Parent"},
            });
    }
}
