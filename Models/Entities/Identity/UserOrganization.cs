using Microsoft.AspNetCore.Identity;

namespace Models.Entities.Identity;

public class UserOrganization
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int OrganizationId { get; set; }

    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization? Organization { get; set; }

    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public virtual User? User { get; set; }
}