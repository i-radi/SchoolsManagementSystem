namespace SMS.VModels.DTOS;

public class AddSeasonDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsCurrent { get; set; }
    public int SchoolId { get; set; }
}
