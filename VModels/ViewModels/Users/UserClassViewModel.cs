namespace VModels.ViewModels;

public class UserClassViewModel
{
    public int Id { get; set; }
    public int ClassroomId { get; set; }
    public virtual Classroom? Classroom { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public int UserTypeId { get; set; }
    public virtual UserType? UserType { get; set; }
    public int SeasonId { get; set; }
    public virtual Season? Season { get; set; }
}
