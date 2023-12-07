namespace VModels.ViewModels.Attendances;

public class ActivityAttendanceViewModel
{
    public int ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public List<UserAttendance> Classes { get; set; } = new();
    public List<InstanceAttendance> AllActivityInstances { get; set; } = new();
}

public class UserAttendance
{
    public int ClassId { get; set; }
    public string? ClassName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int UserTypeId { get; set; }
    public string? UserType { get; set; }
    public List<int> InstanceIds { get; set; } = new();
}

public class InstanceAttendance
{
    public int InstanceId { get; set; }
    public string? InstanceName { get; set; }
    public DateTime InstanceDate { get; set; }
}