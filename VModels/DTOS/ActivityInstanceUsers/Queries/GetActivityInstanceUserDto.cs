namespace VModels.DTOS;

public class GetActivityInstanceUserDto
{
    public int Id { get; set; }
    public int ActivityInstanceId { get; set; }
    public int UserId { get; set; }
    public string? Note { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
