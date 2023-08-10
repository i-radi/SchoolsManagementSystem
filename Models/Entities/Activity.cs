using Models.Entities.Identity;

namespace Models.Entities;
public class Activity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public int SchoolId { get; set; }
    [ForeignKey(nameof(SchoolId))]
    public virtual School? School { get; set; }

    public int RoleId { get; set; }
    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }
}
