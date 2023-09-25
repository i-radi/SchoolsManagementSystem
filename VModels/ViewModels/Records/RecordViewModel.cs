using VModels.ViewModels;

namespace Models.Entities;

public class RecordViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool Available { get; set; }
    public int Order { get; set; }
    public double Points { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool ForTeachers { get; set; }
    public bool ForStudents { get; set; }
    public int SchoolId { get; set; }
    public int OrganizationId { get; set; }
    public virtual SchoolViewModel? School { get; set; }
    public virtual ICollection<RecordClassViewModel> RecordClasses { get; set; } = new HashSet<RecordClassViewModel>();
    public virtual ICollection<UserRecordViewModel> UserRecords { get; set; } = new HashSet<UserRecordViewModel>();
}
