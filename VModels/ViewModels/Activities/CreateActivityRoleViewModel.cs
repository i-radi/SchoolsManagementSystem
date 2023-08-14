namespace VModels.ViewModels;

public class CreateActivityRoleViewModel
{
    public string ActivityTitle { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public int ActivityId { get; set; }
    public int SchoolId { get; set; }
    public int OrganizationId { get; set; }
}
