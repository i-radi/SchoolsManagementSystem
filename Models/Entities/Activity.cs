using Models.Entities.Identity;

namespace Models.Entities;
public class Activity
{
    public int ActivityId { get; set; }
    public string Title { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User? User { get; set; }

    public int ClassroomId { get; set; }
    public ClassRoom? ClassRoom { get; set; }

    public int SeasonId { get; set; }
    public Season? Season { get; set; }

    public ICollection<Attendance> Attendances { get; set; } = new HashSet<Attendance>();
}
