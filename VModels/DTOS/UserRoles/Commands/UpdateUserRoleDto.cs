namespace VModels.DTOS;

public class UpdateUserRoleDto
{
    public int Id { get; set; }
    public int? OrganizationId { get; set; }
    public int? SchoolId { get; set; }
    public int? ActivityId { get; set; }
    public int RoleId { get; set; }
    public int UserId { get; set; }
}
