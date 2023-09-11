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
    public virtual School? School { get; set; }
    public virtual ICollection<RecordClass> RecordClasses { get; set; } = new HashSet<RecordClass>();
    public virtual ICollection<UserRecord> UserRecords { get; set; } = new HashSet<UserRecord>();
}
