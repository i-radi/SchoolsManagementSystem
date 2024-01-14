namespace VModels.DTOS;

public class AddGradeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public int SchoolId { get; set; }
}
