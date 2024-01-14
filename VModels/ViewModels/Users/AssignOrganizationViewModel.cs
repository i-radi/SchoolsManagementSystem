namespace VModels.ViewModels;

public class AssignOrganizationViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public List<int> SelectedOrganizationIds { get; set; } = new List<int>();
    public SelectList? OrganizationOptions { get; set; }
}

