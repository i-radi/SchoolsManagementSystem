namespace Models.Entities;

public class CourseDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public ContentType ContentType { get; set; }
    public int CourseId { get; set; }
    [ForeignKey(nameof(CourseId))]
    public virtual Course? Course { get; set; }
}

public enum ContentType
{
    Text,
    Link,
    Attachment
}