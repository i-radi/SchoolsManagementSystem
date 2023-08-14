using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity;

public class Role : IdentityRole<int>
{
    public int? OrganizationId { get; set; }
    public int? SchoolId { get; set; }
    public int? ActivityId { get; set; }
    [ForeignKey(nameof(ActivityId))]
    public virtual Activity? Activity { get; set; }
}