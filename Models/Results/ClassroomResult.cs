namespace Models.Results;

public class ClassroomResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public int Order { get; set; }
    public string? PicturePath { get; set; }
    public string? TeacherImagePath { get; set; }
    public string? StudentImagePath { get; set; }
    public UserTypeResult? UserType { get; set; }
    public GradeResult? Grade { get; set; }
    public SeasonResult? Season { get; set; }
    public SchoolResult? School { get; set; }
    public OrganizationResult? Organization { get; set; }
}

public class GradeResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class SchoolResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PicturePath { get; set; } = string.Empty;
}

public class OrganizationResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PicturePath { get; set; } = string.Empty;
}

public class UserTypeResult
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SeasonResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsCurrent { get; set; }
}
