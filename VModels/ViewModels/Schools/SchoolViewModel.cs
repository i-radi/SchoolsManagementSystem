namespace VModels.ViewModels;

public class SchoolViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
    public virtual Organization? Organization { get; set; }
    public virtual ICollection<Season> Seasons { get; set; } = new HashSet<Season>();
    public virtual ICollection<Activity> Activities { get; set; } = new HashSet<Activity>();
}
