using System.ComponentModel.DataAnnotations;

namespace SMS.VModels.DTOS;

public class RegisterDto
{
    public RegisterDto(
        string name,
        string userName,
        string email,
        string password,
        string phoneNumber,
        string role
        )
    {
        Name = name;
        UserName = userName;
        Email = email;
        Password = password;
        PhoneNumber = phoneNumber;
        Role = role;
    }

    [StringLength(100)]
    public string Name { get; set; }

    [StringLength(50)]
    [Required]
    public string UserName { get; set; }

    [StringLength(128)]
    [Required]
    public string Email { get; set; }

    [StringLength(50)]
    [Required]
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Role { get; set; }
}
