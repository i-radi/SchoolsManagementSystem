using VModels.ViewModels;

namespace Models.Entities;

public class RecordClassViewModel
{
    public int Id { get; set; }
    public int RecordId { get; set; }
    public virtual RecordViewModel? Record { get; set; }
    public int ClassroomId { get; set; }
    public virtual ClassroomViewModel? Classroom { get; set; }
}
