namespace VModels.ViewModels;

public class MemberViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public int? SchoolId { get; set; }
    public string School { get; set; } = string.Empty;
    public int? OrganizationId { get; set; }
    public string Organization { get; set; } = string.Empty;
    public int ClassRoomId { get; set; }
    public string ClassRoom { get; set; } = string.Empty;
    public int UserTypeId { get; set; }
    public string UserType { get; set; } = string.Empty;
    public int SeasonId { get; set; }
    public string Season { get; set; } = string.Empty;


}
