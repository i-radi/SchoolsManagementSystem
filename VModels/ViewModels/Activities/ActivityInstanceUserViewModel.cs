

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
public class ActivityInstanceUserDataViewModel
{
    public int Id { get; set; }
    public int ActivityInstanceId { get; set; }
    public List<int>  UserIds { get; set; }
    public List<ActivityInstanceUsersDataViewMode>  activityInstanceUsersDataViewModels{ get; set; } = new List<ActivityInstanceUsersDataViewMode>();   
    public DateTime CreatedDate { get; set; }
    public bool IsSelectAll { get; set; }

}
public class ActivityInstanceUsersDataViewMode
{
    public int UserId { get; set; }
    public string UserName { get; set; }=string.Empty;  
    public string? Note { get; set; } = string.Empty;
    public bool IsSelected { get; set; }

}