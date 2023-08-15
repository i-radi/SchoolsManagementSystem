using Microsoft.AspNetCore.Http;

namespace VModels.ViewModels;

public class ClassroomFormViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public int GradeId { get; set; }
    public IFormFile Picture { get; set; }
    public string PicturePath { get; set; }
}
