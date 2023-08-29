using Microsoft.AspNetCore.Http;

namespace VModels.ViewModels;

public class UserFormViewModel
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IFormFile? ProfilePicture { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? SchoolUniversityJob { get; set; }
    public string? FirstMobile { get; set; }
    public string? SecondMobile { get; set; }
    public string? FatherMobile { get; set; }
    public string? MotherMobile { get; set; }
    public string? MentorName { get; set; }
    public string GpsLocation { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? NationalID { get; set; }
    public string? ParticipationQRCodePath { get; set; }
    public int? OrganizationId { get; set; }
}
