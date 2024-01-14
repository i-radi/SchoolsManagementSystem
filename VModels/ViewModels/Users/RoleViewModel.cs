namespace VModels.ViewModels;

public class RoleViewModel
{
    public virtual ICollection<UserRoleViewModel>? UserRoles { get; set; } = new HashSet<UserRoleViewModel>();
}