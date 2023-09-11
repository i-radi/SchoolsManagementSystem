using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class GetCourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? CourseDate { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? CreatedDate { get; set; } = DateTime.Now;
    public int OrganizationId { get; set; }
    public int SchoolId { get; set; }
    public virtual School? School { get; set; }
    public string? Content { get; set; } = string.Empty;
    public ContentType ContentType { get; set; }
    public IFormFile? Attachment { get; set; }
}
