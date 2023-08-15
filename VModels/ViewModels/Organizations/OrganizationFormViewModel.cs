using Microsoft.AspNetCore.Http;

namespace VModels.ViewModels;

public class OrganizationFormViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public IFormFile Picture { get; set; }
    public string PicturePath { get; set; }
}
