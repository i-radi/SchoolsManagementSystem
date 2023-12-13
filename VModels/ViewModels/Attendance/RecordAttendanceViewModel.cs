namespace VModels.ViewModels.Attendances;

public class RecordAttendanceViewModel
{
    public int RecordId { get; set; }
    public string? RecordName { get; set; }
    public List<ClassUserAttendance> ClassUsers { get; set; } = new();
    public List<InstanceAttendance> RecordDates { get; set; } = new();
}

public class ClassUserAttendance
{
    public int ClassId { get; set; }
    public string? ClassName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int UserTypeId { get; set; }
    public string? UserType { get; set; }
    public List<DateTime> RecordDates { get; set; } = new();
}