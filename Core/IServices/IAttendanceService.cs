using VModels.DTOS.ActivityInstanceUsers.Queries;
using VModels.ViewModels.Attendances;

namespace Core.IServices;

public interface IAttendanceService
{
    Task<ActivityAttendanceViewModel?> GetByActivityId(int id);
    Task<RecordAttendanceViewModel?> GetByRecordId(int id);
    Task<Result<GetActivityInstanceWithUsersDto>> GetByActivityInstanceId(int activityInstanceId);
}
