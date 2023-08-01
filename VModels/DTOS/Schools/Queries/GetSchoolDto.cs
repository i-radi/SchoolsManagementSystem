namespace VModels.DTOS;

public class GetSchoolDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
}
