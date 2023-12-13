using VModels.Utilities;

namespace VModels.ViewModels;

public class CopyUserClassViewModel
{
    public int FromClassroomId { get; set; }
    public int FromSeasonId { get; set; }
    public int ToClassroomId { get; set; }
    public int ToSeasonId { get; set; }
    public string SelectedUserIds { get; set; }
    public PaginatedList<UserClassViewModel> From { get; set; }
}
