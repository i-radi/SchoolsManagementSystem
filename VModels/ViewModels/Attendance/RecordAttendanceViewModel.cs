namespace VModels.ViewModels.Attendances;

public class RecordAttendanceViewModel
{
    public int RecordId { get; set; }
    public string? RecordName { get; set; }
    public List<UserAttendance> ClassUsers { get; set; } = new();
    public List<InstanceAttendance> RecordDates { get; set; } = new();
}