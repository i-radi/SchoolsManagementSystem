namespace VModels.DTOS;

public class GetProfileDto
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Gender { get; set; }
    public DateTime? Birthdate { get; set; }
    public string? PositionType { get; set; }
    public string? SchoolUniversityJob { get; set; }
    public string? FirstMobile { get; set; }
    public string? SecondMobile { get; set; }
    public string? FatherMobile { get; set; }
    public string? MotherMobile { get; set; }
    public string? MentorName { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? GpsLocation { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? NationalID { get; set; }
    public int ParticipationNumber { get; set; }
    public List<UserRolesDto> Roles { get; set; } = new List<UserRolesDto>();
    public List<OrganizationDto> Organizations { get; set; } = new List<OrganizationDto>();
}

public class UserRolesDto
{
    public int UserRoleId { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public int? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public int? SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public int? ActivityId { get; set; }
    public string? ActivityName { get; set; }
}

public class OrganizationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public static class GetProfileDtoMapping
{
    public static GetProfileDto ToGetProfileDto(this User user)
    {
        var userProfile = new GetProfileDto()
        {
            Address = user.Address,
            Birthdate = user.Birthdate,
            Email = user.Email,
            Name = user.Name,
            FatherMobile = user.FatherMobile,
            FirstMobile = user.FirstMobile,
            Gender = user.Gender,
            GpsLocation = user.GpsLocation,
            MentorName = user.MentorName,
            MotherMobile = user.MotherMobile,
            NationalID = user.NationalID,
            Notes = user.Notes,
            PositionType = user.PositionType,
            PhoneNumber = user.PhoneNumber,
            ParticipationNumber = user.ParticipationNumber,
            ProfilePicturePath = user.ProfilePicturePath,
            SchoolUniversityJob = user.SchoolUniversityJob,
            SecondMobile = user.SecondMobile,
            UserName = user.UserName,
        };

        foreach (var userRole in user.UserRoles)
        {
            userProfile.Roles.Add(new UserRolesDto()
            {
                UserRoleId = userRole.Id,
                RoleId = userRole.RoleId,
                RoleName = userRole.Role?.Name,
                OrganizationId = userRole.OrganizationId,
                OrganizationName = userRole.Organization?.Name,
                SchoolId = userRole.SchoolId,
                SchoolName = userRole.School?.Name,
                ActivityId = userRole.ActivityId,
                ActivityName = userRole.Activity?.Name
            });
        }

        foreach (var userorg in user.UserOrganizations)
        {
            userProfile.Organizations.Add(new OrganizationDto()
            {
                Id = userorg.OrganizationId,
                Name = userorg.Organization?.Name
            });
        }

        return userProfile;
    }
}