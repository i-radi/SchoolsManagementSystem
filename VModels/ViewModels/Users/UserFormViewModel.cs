using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VModels.ViewModels;

public class UserFormViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public string? ProfilePicturePath { get; set; }
}
