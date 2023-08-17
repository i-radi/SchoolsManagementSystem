using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class GetActivityClassroomDto
{
    public int Id { get; set; }
    public string Activity { get; set; } = string.Empty;
    public string Classroom { get; set; } = string.Empty;
}
