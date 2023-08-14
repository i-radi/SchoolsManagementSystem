using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity;

public class Role : IdentityRole<int>
{
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}