namespace VModels.DTOS;

public class AddActivityTimeDto
{
    public int ActivityId { get; set; }
    public string Day { get; set; } = string.Empty;
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
    public string? Body { get; set; }
}
