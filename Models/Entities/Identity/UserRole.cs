using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity;

public class UserRole : IdentityUserRole<int>
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int? OrganizationId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization? Organization { get; set; }
    public int? SchoolId { get; set; }
    [ForeignKey(nameof(SchoolId))]
    public virtual School? School { get; set; }
    public int? ActivityId { get; set; }
    [ForeignKey(nameof(ActivityId))]
    public virtual Activity? Activity { get; set; }
    public override int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public virtual Role? Role { get; set; }

    public override int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}