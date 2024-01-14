namespace Models.Results;

public class RoleResult
{
    public int UserRoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? OrganizationId { get; set; }
    public string Organization { get; set; } = string.Empty;
    public int? SchoolId { get; set; }
    public string School { get; set; } = string.Empty;
    public int? ActivityId { get; set; }
    public string Activity { get; set; } = string.Empty;
}
