namespace SMS.VModels.DTOS;

public class UpdateGradeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
}
