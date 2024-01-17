using VModels.DTOS.Report;

namespace VModels.DTOS;

public class GetSchoolDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Organization { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
    public List<GradesDto> Grades { get; set; } = new List<GradesDto>();
 
}
