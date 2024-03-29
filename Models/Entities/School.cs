﻿using Models.Entities.Identity;

namespace Models.Entities;

public class School
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PicturePath { get; set; } = string.Empty;
    public int Order { get; set; }
    public int OrganizationId { get; set; }
    [ForeignKey(nameof(OrganizationId))]
    public virtual Organization? Organization { get; set; }
    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    public virtual ICollection<Season> Seasons { get; set; } = new HashSet<Season>();
    public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
    public virtual ICollection<Activity> Activities { get; set; } = new HashSet<Activity>();
    public virtual ICollection<Record> Records { get; set; } = new HashSet<Record>();
    public virtual ICollection<Course> Courses { get; set; } = new HashSet<Course>();
}
