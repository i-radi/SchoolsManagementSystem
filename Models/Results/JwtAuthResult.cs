namespace Models.Results;

public class JwtAuthResult
{
    public int Id { get; set; }
    public bool IsSuperAdmin { get; set; }
    public UserInformation UserInformations { get; set; } = new UserInformation();
    public List<RoleResult> Roles { get; set; } = new List<RoleResult>();
    public List<ClassroomResult> Classrooms { get; set; } = new List<ClassroomResult>();
    public bool IsAuthenticated { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiryDate { get; set; }
    public Guid RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryDate { get; set; }
}

public class UserInformation
{
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
}
