namespace VModels.ViewModels;

public class GradeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public int SchoolId { get; set; }
    public virtual SchoolViewModel? School { get; set; }
    public virtual ICollection<ClassroomViewModel> Classrooms { get; set; } = new HashSet<ClassroomViewModel>();
}
