using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class RegisterDto
{
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    [Required]
    public string UserName { get; set; } = string.Empty;

    [StringLength(128)]
    [Required]
    public string Email { get; set; } = string.Empty;

    [StringLength(50)]
    [Required]
    public string Password { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int? OrganizationId { get; set; }
    public int? SchoolId { get; set; }
}
