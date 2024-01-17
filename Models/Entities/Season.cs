namespace Models.Entities;

public class Season
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsCurrent { get; set; }
    public int SchoolId { get; set; }
    [ForeignKey(nameof(SchoolId))]
    public virtual School School { get; set; }
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<ActivityInstance> ActivityInstances { get; set; } = new HashSet<ActivityInstance>();
}
