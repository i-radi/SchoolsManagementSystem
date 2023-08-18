namespace VModels.ViewModels;

public class GradeViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public virtual School? School { get; set; }
    public virtual ICollection<ClassRoom> ClassRooms { get; set; } = new HashSet<ClassRoom>();
}
