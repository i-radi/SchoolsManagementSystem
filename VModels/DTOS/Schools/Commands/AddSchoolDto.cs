namespace VModels.DTOS;

public class AddSchoolDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
}
