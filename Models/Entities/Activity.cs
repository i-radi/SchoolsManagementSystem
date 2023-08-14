using Models.Entities.Identity;

namespace Models.Entities;
public class Activity
{
    public Activity()
    {
        
    }

    public Activity(string title,int schoolId)
    {
        Title = title;
        SchoolId = schoolId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public int SchoolId { get; set; }
    [ForeignKey(nameof(SchoolId))]
    public virtual School? School { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}
