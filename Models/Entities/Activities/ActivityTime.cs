namespace Models.Entities;

public class ActivityTime
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ActivityId { get; set; }
    [ForeignKey(nameof(ActivityId))]
    public virtual Activity? Activity { get; set; }
    public string Day { get; set; } = string.Empty;
    public DateTime FromTime { get; set; }
    public DateTime ToTime { get; set; }
    public string? Body { get; set; }
}
