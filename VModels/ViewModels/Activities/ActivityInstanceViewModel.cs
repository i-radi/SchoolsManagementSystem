﻿using System.ComponentModel.DataAnnotations;

namespace VModels.ViewModels;

public class ActivityInstanceViewModel
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public virtual ActivityViewModel? Activity { get; set; }
    public string Name { get; set; } = string.Empty;
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime CreatedDate { get; set; }
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime ForDate { get; set; }
    public bool IsLocked { get; set; }
    public int SeasonId { get; set; }
    public virtual SeasonViewModel? Season { get; set; }
    public virtual ICollection<ActivityInstanceUserViewModel> ActivityInstanceUsers { get; set; } = new HashSet<ActivityInstanceUserViewModel>();
}
