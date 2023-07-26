using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS.Models.Entities.Identity;

public class User : IdentityUser<int>
{
    public string Name { get; set; } = string.Empty;
    public int? OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization? Organization { get; set; }
    public string PlainPassword { get; set; } = string.Empty;
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
}
