using System.ComponentModel.DataAnnotations;

namespace SMS.VModels.DTOS;

public class LoginDto
{
    public LoginDto(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required]
    public string Email { get; }

    [Required]
    public string Password { get; }
}
