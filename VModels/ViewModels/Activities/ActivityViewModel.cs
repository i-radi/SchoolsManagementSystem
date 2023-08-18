using Models.Entities.Identity;

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
    public virtual School? School { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<ActivityClassroom> ActivityClasses { get; set; } = new HashSet<ActivityClassroom>();
    public virtual ICollection<ActivityTime> ActivityTimes { get; set; } = new HashSet<ActivityTime>();
    public virtual ICollection<ActivityInstance> ActivityInstances { get; set; } = new HashSet<ActivityInstance>();
}
