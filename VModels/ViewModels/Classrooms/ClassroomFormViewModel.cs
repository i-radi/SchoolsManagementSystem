using Microsoft.AspNetCore.Http;

namespace VModels.ViewModels;

public class ClassroomFormViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int Order { get; set; }
    public int GradeId { get; set; }
    public IFormFile? TeacherImage { get; set; }
    public string? TeacherImagePath { get; set; }
    public IFormFile? StudentImage { get; set; }
    public string? StudentImagePath { get; set; }
    public IFormFile? Picture { get; set; }
    public string? PicturePath { get; set; }
}
