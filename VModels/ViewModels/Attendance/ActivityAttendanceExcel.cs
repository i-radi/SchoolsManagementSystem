namespace VModels.ViewModels.Attendances;

public class ActivityAttendanceViewExcel
{
    public int ActivityId { get; set; }
    public string? ActivityName { get; set; }
    public int ClassId { get; set; }
    public string? ClassName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public int UserTypeId { get; set; }
    public string? UserType { get; set; }
    public List<string> Instances { get; set; } = new();
    public string Total { get; set; }
}