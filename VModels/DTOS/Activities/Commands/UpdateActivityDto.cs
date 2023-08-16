namespace VModels.DTOS;

public class UpdateActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
