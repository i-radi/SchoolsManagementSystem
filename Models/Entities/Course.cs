namespace Models.Entities;

public class Course
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime? CourseDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int SchoolId { get; set; }
    [ForeignKey(nameof(SchoolId))]
    public virtual School? School { get; set; }
    public virtual CourseDetails? CourseDetails { get; set; }

}
