using System.Runtime.CompilerServices;

namespace VModels.DTOS;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? SchoolUniversityJob { get; set; }
    public string? FirstMobile { get; set; }
    public string? SecondMobile { get; set; }
    public string? FatherMobile { get; set; }
    public string? MotherMobile { get; set; }
    public string? MentorName { get; set; }
    public string? GpsLocation { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? NationalID { get; set; }
}

public static class UserMapping
{
    public static User MapUpdateUserDto(this User user, UpdateUserDto dto)
    {
        user.Name = dto.Name;
        user.PhoneNumber = dto.PhoneNumber;
        user.Address = dto.Address;
        user.Gender = dto.Gender;
        user.Birthdate = dto.Birthdate;
        user.SchoolUniversityJob = dto.SchoolUniversityJob;
        user.FirstMobile = dto.FirstMobile;
        user.SecondMobile = dto.SecondMobile;
        user.FatherMobile = dto.FatherMobile;
        user.MotherMobile = dto.MotherMobile;
        user.MentorName = dto.MentorName;
        user.GpsLocation = dto.GpsLocation;
        user.Notes = dto.Notes;
        user.NationalID = dto.NationalID;

        return user;
    }
}

