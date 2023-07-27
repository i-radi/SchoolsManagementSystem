namespace SMS.VModels.DTOS;

public class UpdateSchoolDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
}
