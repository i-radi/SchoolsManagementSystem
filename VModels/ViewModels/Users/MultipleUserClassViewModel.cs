namespace VModels.ViewModels;

public class MultipleUserClassViewModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public virtual Organization? Organization { get; set; }

    public int SchoolId { get; set; }
    public virtual School? School { get; set; }

    public int GradeId { get; set; }
    public virtual Grade? Grade { get; set; }

    public int ClassroomId { get; set; }
    public virtual Classroom? Classroom { get; set; }

    public List<int> SelectedUserIds { get; set; }
    public virtual User? User { get; set; }
    public int UserTypeId { get; set; }
    public virtual UserType? UserType { get; set; }
    public int SeasonId { get; set; }
    public virtual Season? Season { get; set; }
}
