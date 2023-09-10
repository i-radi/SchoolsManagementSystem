using Models.Entities.Identity;

namespace Models.Entities;
public class UserRecord
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int RecordId { get; set; }
    [ForeignKey(nameof(RecordId))]
    public virtual Record? Record { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    public DateTime? CreateDate { get; set; } = DateTime.Now;
    public bool IsDone { get; set; }
    public DateTime? DoneDate { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
}
