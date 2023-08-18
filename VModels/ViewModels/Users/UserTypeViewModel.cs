namespace VModels.ViewModels;

public class UserTypeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
}
