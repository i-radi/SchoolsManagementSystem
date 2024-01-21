using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class LoginDto
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
