using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities.Identity;

namespace Persistance.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.UserName).IsUnique();
        builder.ToTable(tb => tb.HasTrigger("trg_SetParticipationNumber"));

        builder.HasData(
            new List<User>
            {
                new User()
                {
                    Id = 1,
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@sms.com",
                    NormalizedEmail = "ADMIN@SMS.COM",
                    Name = "Admin User",
                    PlainPassword = "123456",
                    RefreshToken = Guid.NewGuid(),
                    RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                    ProfilePicturePath = "emptyAvatar.png",
                    PasswordHash = "AQAAAAIAAYagAAAAEEEvuzV4blWESxZcSEnPlgLgae4bQZgB6A29NU/zj9FS91zzZKF9odfHtexpQHlzGg==",
                    SecurityStamp = "XCGDJZV44O4PZ47TD7MFQCD27H5DO4MB",
                    ConcurrencyStamp = "721a882b-410e-46e0-b878-625aaa8bbd75"
                }
            });
    }
}
