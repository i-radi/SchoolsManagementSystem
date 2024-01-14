using System.ComponentModel.DataAnnotations;

namespace VModels.ViewModels;

public class SeasonViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime From { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime To { get; set; }
    public bool IsCurrent { get; set; }
    public int SchoolId { get; set; }
    public virtual SchoolViewModel? School { get; set; }
    public virtual ICollection<UserClassViewModel> UserClasses { get; set; } = new HashSet<UserClassViewModel>();
    public virtual ICollection<ActivityInstanceViewModel> ActivityInstances { get; set; } = new HashSet<ActivityInstanceViewModel>();
}
