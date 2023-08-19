namespace VModels.DTOS;

public class UpdateClassroomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int GradeId { get; set; }
}
