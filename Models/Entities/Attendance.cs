namespace Models.Entities;

public class Attendance
{
    public int AttendanceId { get; set; }
    public DateTime Date { get; set; }

    public int ActivityId { get; set; }
    public Activity? Activity { get; set; }
}
