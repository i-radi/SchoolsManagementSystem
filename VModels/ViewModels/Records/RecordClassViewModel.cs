namespace Models.Entities;

public class RecordClassViewModel
{
    public int Id { get; set; }
    public int RecordId { get; set; }
    public virtual Record? Record { get; set; }
    public int ClassroomId { get; set; }
    public virtual Classroom? Classroom { get; set; }
}
