namespace VModels.DTOS;

public class GetUserClassDto
{
    public int Id { get; set; }
    public string Class { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    public string Season { get; set; } = string.Empty;
}
