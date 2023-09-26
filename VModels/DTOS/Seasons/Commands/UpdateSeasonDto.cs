namespace VModels.DTOS;

public class UpdateSeasonDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsCurrent { get; set; }
    public int SchoolId { get; set; }
}
