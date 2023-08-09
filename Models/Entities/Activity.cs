using Models.Entities.Identity;

namespace Models.Entities;
public class Activity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public int AdminId { get; set; }
    [ForeignKey(nameof(AdminId))]
    public virtual User? User { get; set; }

    public int ClassroomId { get; set; }
    [ForeignKey(nameof(ClassroomId))]
    public virtual ClassRoom? ClassRoom { get; set; }

    public int SeasonId { get; set; }
    [ForeignKey(nameof(SeasonId))]
    public virtual Season? Season { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
}
