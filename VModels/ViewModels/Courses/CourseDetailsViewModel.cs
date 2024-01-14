namespace Models.Entities;

public class CourseDetailsViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public ContentType ContentType { get; set; }
    public int CourseId { get; set; }
    public virtual CourseViewModel? Course { get; set; }
}