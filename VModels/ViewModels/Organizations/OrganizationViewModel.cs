namespace VModels.ViewModels;

public class OrganizationViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
    public virtual ICollection<SchoolViewModel> Schools { get; set; } = new HashSet<SchoolViewModel>();
    public virtual ICollection<UserViewModel> Users { get; set; } = new HashSet<UserViewModel>();
}
