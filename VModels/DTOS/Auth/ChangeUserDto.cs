using System.ComponentModel.DataAnnotations;

namespace VModels.DTOS;

public class ChangeUserDto
{
    public ChangeUserDto(int id, string email, string password)
    {
        UserId = id;
        Email = email;
        Password = password;
    }
    public int UserId { get; set; }
    [Required]
    public string Email { get; } = string.Empty;
    [Required]
    public string Password { get; } = string.Empty;
}
