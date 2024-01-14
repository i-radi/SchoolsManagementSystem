namespace VModels.DTOS;

public class UserRoleRequest
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public int? OrganizationId { get; set; }
    public int? SchoolId { get; set; }
    public int? ActivityId { get; set; }
}

public static class UserRoleMapping
{
    public static AddUserRoleDto ToDto(this UserRoleRequest request, int userId)
    {
        return new AddUserRoleDto
        {
            UserId = userId,
            RoleId = request.RoleId,
            OrganizationId = request.OrganizationId,
            SchoolId = request.SchoolId,
            ActivityId = request.ActivityId
        };
    }
}
