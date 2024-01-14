using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class ChangeUserPasswordDto
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    [Required]
    public string NewPassword { get; set; } = string.Empty;
}
