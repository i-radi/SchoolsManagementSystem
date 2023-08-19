namespace VModels.ViewModels;


public class ClassroomViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
    public int GradeId { get; set; }
    public virtual Grade? Grade { get; set; }
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<ActivityClassroom> ActivityClassrooms { get; set; } = new HashSet<ActivityClassroom>();
}
