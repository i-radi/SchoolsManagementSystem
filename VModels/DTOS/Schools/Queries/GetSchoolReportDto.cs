namespace VModels.DTOS.Report;

public class GetSchoolReportDto
{
    public int SchoolId { get; set; }
    public string School { get; set; } = string.Empty;
    public string SchoolDescription { get; set; } = string.Empty;
    public string Organization { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
    public int SeasonId { get; set; }
    public string Season { get; set; } = string.Empty;
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsCurrent { get; set; }
    public List<GradesDto>? Grades { get; set; }
}
public class GradesDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public List<ClassroomDto>? Classrooms { get; set; }
}
public class ClassroomDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? PicturePath { get; set; }
    public string? TeacherImagePath { get; set; }
    public string? StudentImagePath { get; set; }
    public List<UserDto>? Users { get; set; }
}
public class UserDto
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
    public int UserTypeId { get; set; }
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