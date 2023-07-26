namespace SMS.VModels.DTOS.Users.Queries;

public class GetUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string Role { get; set; } = String.Empty;
}
