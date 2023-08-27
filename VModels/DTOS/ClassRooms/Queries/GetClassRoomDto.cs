namespace VModels.DTOS;

public class GetClassroomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? PicturePath { get; set; }
    public string? TeacherImagePath { get; set; }
    public string? StudentImagePath { get; set; }
    public string Grade { get; set; } = string.Empty;
}
