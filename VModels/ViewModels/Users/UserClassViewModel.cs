namespace VModels.ViewModels;

public class UserClassViewModel
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public virtual OrganizationViewModel? Organization { get; set; }

    public int SchoolId { get; set; }
    public virtual SchoolViewModel? School { get; set; }

    public int GradeId { get; set; }
    public virtual GradeViewModel? Grade { get; set; }

    public int ClassroomId { get; set; }
    public virtual ClassroomViewModel? Classroom { get; set; }

    public int UserId { get; set; }
    public virtual UserViewModel? User { get; set; }
    public int UserTypeId { get; set; }
    public virtual UserTypeViewModel? UserType { get; set; }
    public int SeasonId { get; set; }
    public virtual SeasonViewModel? Season { get; set; }
}
