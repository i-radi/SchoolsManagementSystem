namespace VModels.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
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
    public string GpsLocation { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? NationalID { get; set; }
    public string? ParticipationQRCodePath { get; set; }
    public string Organization { get; set; } = string.Empty;
    public string PlainPassword { get; set; } = string.Empty;
    public string ProfilePicturePath { get; set; } = string.Empty;
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<ActivityInstanceUser> ActivityInstanceUsers { get; set; } = new HashSet<ActivityInstanceUser>();
}
