using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? SchoolUniversityJob { get; set; }
    public string? FirstMobile { get; set; }
    public string? SecondMobile { get; set; }
    public string? FatherMobile { get; set; }
    public string? MotherMobile { get; set; }
    public string? MentorName { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? GpsLocation { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? NationalID { get; set; }
    public int ParticipationNumber { get; set; }
    public string? ParticipationQRCodePath { get; set; }
    public string PlainPassword { get; set; } = string.Empty;
    public string? AccessToken { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryDate { get; set; }
    public virtual ICollection<UserOrganization> UserOrganizations { get; set; } = new HashSet<UserOrganization>();
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<ActivityInstanceUser> ActivityInstanceUsers { get; set; } = new HashSet<ActivityInstanceUser>();
}
