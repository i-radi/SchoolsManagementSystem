﻿namespace VModels.DTOS;

public class GetGradeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Order { get; set; }
    public string School { get; set; } = string.Empty;
}
