namespace VModels.DTOS;

public class AddActivityDto
{
    public string Title { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
