namespace Models.Entities;

public class ActivityInstanceSeason
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ActivityInstanceId { get; set; }
    [ForeignKey(nameof(ActivityInstanceId))]
    public virtual ActivityInstance? ActivityInstance { get; set; }
    public int SeasonId { get; set; }
    [ForeignKey(nameof(SeasonId))]
    public virtual Season? Season { get; set; }
}
