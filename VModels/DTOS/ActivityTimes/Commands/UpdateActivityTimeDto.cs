namespace VModels.DTOS;

public class UpdateActivityTimeDto
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string Day { get; set; } = string.Empty;
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
    public string? Body { get; set; }
}
