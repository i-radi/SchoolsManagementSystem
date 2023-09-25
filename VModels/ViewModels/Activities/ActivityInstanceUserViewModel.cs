

namespace VModels.ViewModels;

public class ActivityInstanceUserViewModel
{
    public int Id { get; set; }
    public int ActivityInstanceId { get; set; }
    public virtual ActivityInstanceViewModel? ActivityInstance { get; set; }
    public int UserId { get; set; }
    public virtual UserViewModel? User { get; set; }
    public string? Note { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
