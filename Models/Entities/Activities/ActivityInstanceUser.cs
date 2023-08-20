using Models.Entities.Identity;

namespace Models.Entities;

public class ActivityInstanceUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ActivityInstanceId { get; set; }
    [ForeignKey(nameof(ActivityInstanceId))]
    public virtual ActivityInstance? ActivityInstance { get; set; }
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    public string? Note { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
