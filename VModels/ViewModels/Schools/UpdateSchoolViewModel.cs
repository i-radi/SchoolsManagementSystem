using Microsoft.AspNetCore.Http;

namespace VModels.ViewModels;

public class UpdateSchoolViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public string PicturePath { get; set; } = string.Empty;
    public int OrganizationId { get; set; }
    public SelectList? OrganizationOptions { get; set; }
    public IFormFile? Picture { get; set; }
}
