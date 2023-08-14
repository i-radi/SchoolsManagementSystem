using Microsoft.AspNetCore.Mvc.Rendering;

namespace VModels.ViewModels.Users;

public class CreateRoleViewModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public SelectList? RoleOptions { get; set; }
    public int? OrganizationId { get; set; }
    public SelectList? OrganizationOptions { get; set; }
    public int? SchoolId { get; set; }
    public SelectList? SchoolOptions { get; set; }

    public int? ActivityId { get; set; }
    public SelectList? ActivityOptions { get; set; }
}
