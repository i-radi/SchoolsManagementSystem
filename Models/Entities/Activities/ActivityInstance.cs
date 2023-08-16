using Models.Entities.Identity;
using System;

namespace Models.Entities;

public class ActivityInstance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ActivityId { get; set; }
    [ForeignKey(nameof(ActivityId))]
    public virtual Activity? Activity { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ForDate { get; set; }
    public bool IsLocked { get; set; }
    public virtual ICollection<ActivityInstanceUser> ActivityInstanceUsers { get; set; } = new HashSet<ActivityInstanceUser>();
    public virtual ICollection<ActivityInstanceSeason> ActivitySeasons { get; set; } = new HashSet<ActivityInstanceSeason>();
}
