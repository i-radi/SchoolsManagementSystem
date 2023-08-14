namespace VModels.DTOS.Auth;

public class RoleDto
{
    public int roleId { get; set; }
    public int? organizationId { get; set; }
    public int? schoolId { get; set; }
    public int? activityId { get; set; }
}
