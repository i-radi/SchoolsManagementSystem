namespace VModels.ViewModels;


public class ClassroomViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? TeacherImagePath { get; set; }
    public string? StudentImagePath { get; set; }
    public string? PicturePath { get; set; }
    public int GradeId { get; set; }
    public virtual Grade? Grade { get; set; }
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<ActivityClassroom> ActivityClassrooms { get; set; } = new HashSet<ActivityClassroom>();
}
