namespace VModels.ViewModels;

public class UserRoleViewModel
{
    public int Id { get; set; }
    public int? OrganizationId { get; set; }
    public int? SchoolId { get; set; }
    public int? ActivityId { get; set; }
    public virtual Activity? Activity { get; set; }
    public int RoleId { get; set; }
    public virtual Role? Role { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
}