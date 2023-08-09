using Models.Entities.Identity;

namespace Models.Entities;

public class UserAttendance
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AttendanceId { get; set; }
    [ForeignKey(nameof(AttendanceId))]
    public virtual Attendance? Attendance { get; set;}
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
    public double Grade { get; set; }


}
