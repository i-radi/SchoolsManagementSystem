namespace VModels.ViewModels;

public class SchoolViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public string PicturePath { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
    public virtual OrganizationViewModel? Organization { get; set; }
    public virtual ICollection<SeasonViewModel> Seasons { get; set; } = new HashSet<SeasonViewModel>();
    public virtual ICollection<ActivityViewModel> Activities { get; set; } = new HashSet<ActivityViewModel>();
}
