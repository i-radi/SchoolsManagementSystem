namespace VModels.ViewModels;

public class RoleViewModel
{
    public virtual ICollection<UserRole>? UserRoles { get; set; } = new HashSet<UserRole>();
}