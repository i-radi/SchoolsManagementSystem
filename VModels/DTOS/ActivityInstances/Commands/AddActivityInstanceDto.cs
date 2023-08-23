namespace VModels.DTOS;

public class AddActivityInstanceDto
{
    public int ActivityId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ForDate { get; set; }
    public bool IsLocked { get; set; }
    public int SeasonId { get; set; }
}
