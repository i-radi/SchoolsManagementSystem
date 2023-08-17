namespace VModels.DTOS;

public class GetActivityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string School { get; set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public int Order { get; set; }
    public string? Location { get; set; }
    public bool ForStudents { get; set; }
    public bool ForTeachers { get; set; }
}
