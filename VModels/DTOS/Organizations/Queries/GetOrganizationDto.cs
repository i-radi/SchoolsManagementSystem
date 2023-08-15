namespace VModels.DTOS;

public class GetOrganizationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
}
