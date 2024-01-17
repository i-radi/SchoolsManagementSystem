using Models.Entities.Identity;

namespace Models.Entities;

public class Organization
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string PicturePath { get; set; } = string.Empty;
    public virtual ICollection<School> Schools { get; set; } = new HashSet<School>();
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<UserOrganization> UserOrganizations { get; set; } = new HashSet<UserOrganization>();


}
