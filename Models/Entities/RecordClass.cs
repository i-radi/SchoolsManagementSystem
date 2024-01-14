namespace Models.Entities;

public class RecordClass
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int RecordId { get; set; }
    [ForeignKey(nameof(RecordId))]
    public virtual Record? Record { get; set; }
    public int ClassroomId { get; set; }
    [ForeignKey(nameof(ClassroomId))]
    public virtual Classroom? Classroom { get; set; }
}
