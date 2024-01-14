namespace VModels.ViewModels;
public class ActivityViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public int Order { get; set; }
    public string? Location { get; set; }
    public bool ForStudents { get; set; }
    public bool ForTeachers { get; set; }
    public int SchoolId { get; set; }
    public virtual SchoolViewModel? School { get; set; }
    public virtual ICollection<UserRoleViewModel> UserRoles { get; set; } = new HashSet<UserRoleViewModel>();
    public virtual ICollection<ActivityClassroomViewModel> ActivityClasses { get; set; } = new HashSet<ActivityClassroomViewModel>();
    public virtual ICollection<ActivityTimeViewModel> ActivityTimes { get; set; } = new HashSet<ActivityTimeViewModel>();
    public virtual ICollection<ActivityInstanceViewModel> ActivityInstances { get; set; } = new HashSet<ActivityInstanceViewModel>();
}
