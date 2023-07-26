namespace SMS.VModels.DTOS.Users.Commands;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
}
