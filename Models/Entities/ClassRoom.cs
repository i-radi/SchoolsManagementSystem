﻿namespace Models.Entities;


public class Classroom
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? PicturePath { get; set; }
    public string? TeacherImage { get; set; }
    public string? StudentImage { get; set; }
    public int GradeId { get; set; }

    [ForeignKey(nameof(GradeId))]
    public virtual Grade? Grade { get; set; }
    public virtual ICollection<UserClass> UserClasses { get; set; } = new HashSet<UserClass>();
    public virtual ICollection<ActivityClassroom> ActivityClassrooms { get; set; } = new HashSet<ActivityClassroom>();
}
