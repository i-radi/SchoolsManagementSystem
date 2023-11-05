namespace Models.Helpers;

public class UserClaimModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int OrgId { get; set; } 
    public int SchoolId { get; set; }
    public string Role { get; set; } = string.Empty;
}
