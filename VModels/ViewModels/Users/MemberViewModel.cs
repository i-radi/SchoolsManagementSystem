﻿namespace VModels.ViewModels;

public class MemberViewModel
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProfilePicturePath { get; set; } = string.Empty;
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
    public int? SchoolId { get; set; }
    public string? School { get; set; } = string.Empty;
    public int? OrganizationId { get; set; }
    public string? Organization { get; set; } = string.Empty;
    public int? ClassroomId { get; set; }
    public string? Classroom { get; set; } = string.Empty;
    public int? UserTypeId { get; set; }
    public string? UserType { get; set; } = string.Empty;
    public int? SeasonId { get; set; }
    public string? Season { get; set; } = string.Empty;


}
