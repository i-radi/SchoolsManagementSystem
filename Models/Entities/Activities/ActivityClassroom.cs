namespace Models.Entities;

public class ActivityClassroom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ActivityId { get; set; }
    [ForeignKey(nameof(ActivityId))]
    public virtual Activity? Activity { get; set; }
    public int ClassroomId { get; set; }
    [ForeignKey(nameof(ClassroomId))]
    public virtual Classroom? Classroom { get; set; }
}
