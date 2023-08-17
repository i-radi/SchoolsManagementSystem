namespace VModels.DTOS;

public class AddActivityDto
{
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
    public bool IsAvailable { get; set; }
    public int Order { get; set; }
    public string? Location { get; set; }
    public bool ForStudents { get; set; }
    public bool ForTeachers { get; set; }
}
