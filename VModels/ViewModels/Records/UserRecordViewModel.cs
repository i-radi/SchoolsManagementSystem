using VModels.ViewModels;

namespace Models.Entities;
public class UserRecordViewModel
{
    public int Id { get; set; }
    public int RecordId { get; set; }
    public virtual RecordViewModel? Record { get; set; }
    public int UserId { get; set; }
    public int OrganizationId { get; set; }
    public virtual UserViewModel? User { get; set; }
    public DateTime? CreateDate { get; set; } = DateTime.Now;
    public bool IsDone { get; set; }
    public DateTime? DoneDate { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
}
