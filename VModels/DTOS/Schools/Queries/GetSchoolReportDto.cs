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
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? ProfilePicturePath { get; set; }
    public string? NationalID { get; set; }
    public int UserTypeId { get; set; }
}