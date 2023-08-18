

namespace VModels.ViewModels;

public class ActivityInstanceUserViewModel
{
    public int Id { get; set; }
    public int ActivityInstanceId { get; set; }
    public virtual ActivityInstance? ActivityInstance { get; set; }
    public int UserId { get; set; }
    public virtual User? User { get; set; }
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
