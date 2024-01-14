using Microsoft.AspNetCore.Http;

namespace VModels.ViewModels;

public class BulkUserClassViewModel
{
    public int OrganizationId { get; set; }
    public int SchoolId { get; set; }
    public int GradeId { get; set; }
    public int ClassroomId { get; set; }
    public int UserTypeId { get; set; }
    public int SeasonId { get; set; }
    public IFormFile? Attachment { get; set; }
}
