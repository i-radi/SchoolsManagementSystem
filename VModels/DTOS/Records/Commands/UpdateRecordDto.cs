namespace VModels.DTOS;

public class UpdateRecordDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Available { get; set; }
    public int Order { get; set; }
    public double Points { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool ForTeachers { get; set; }
    public bool ForStudents { get; set; }
    public int SchoolId { get; set; }
}
