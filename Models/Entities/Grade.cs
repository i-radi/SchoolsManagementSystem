namespace Models.Entities;

public class Grade
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public int SchoolId { get; set; }

    [ForeignKey(nameof(SchoolId))]
    public virtual School? School { get; set; }

    public virtual ICollection<Classroom> Classrooms { get; set; } = new HashSet<Classroom>();
}
