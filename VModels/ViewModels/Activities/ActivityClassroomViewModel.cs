namespace VModels.ViewModels;

public class ActivityClassroomViewModel
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public ActivityViewModel? Activity { get; set; }
    public int ClassroomId { get; set; }
    public virtual ClassroomViewModel? Classroom { get; set; }
}
