namespace VModels.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PlainPassword { get; set; } = string.Empty;
    public string ProfilePicturePath { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public Guid RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<ActivityInstanceUser> ActivityInstanceUsers { get; set; } = new HashSet<ActivityInstanceUser>();
}
