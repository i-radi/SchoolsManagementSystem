using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class LoginDto
{
    [Required]
    public string UserNameOrEmail { get; set; }

    [Required]
    public string Password { get; set; }
}
