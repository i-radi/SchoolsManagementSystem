namespace VModels.ViewModels;

public class ActivityClassroomViewModel
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public Activity? Activity { get; set; }
    public int ClassroomId { get; set; }
    public virtual ClassRoom? Classroom { get; set; }
}
