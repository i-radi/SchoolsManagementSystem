namespace VModels.ViewModels;


public class ClassroomViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int Order { get; set; }
    public string? TeacherImagePath { get; set; }
    public string? StudentImagePath { get; set; }
    public string? PicturePath { get; set; }
    public int GradeId { get; set; }
    public virtual GradeViewModel? Grade { get; set; }
    public virtual ICollection<UserClassViewModel> UserClasses { get; set; } = new HashSet<UserClassViewModel>();
    public virtual ICollection<ActivityClassroomViewModel> ActivityClassrooms { get; set; } = new HashSet<ActivityClassroomViewModel>();
}
