
using VModels.ViewModels.Attendances;

namespace Core.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IActivityRepo _activityRepo;

    public AttendanceService(IActivityRepo activitysRepo)
    {
        _activityRepo = activitysRepo;
    }

    public async Task<ActivityAttendanceViewModel?> GetByActivityId(int id)
    {

        var activity = await _activityRepo.GetByIdAsync(id);
        if (activity == null) return null;
        var result = new ActivityAttendanceViewModel
        {
            ActivityId = activity.Id,
            ActivityName = activity.Name
        };

        // columns of report
        var activityInstances = await _activityRepo.GetActivityInstancesWithUserByActivityId(id);
        foreach (var activityInstance in activityInstances)
        {
            result.ActivityInstances.Add(new InstanceAttendance
            {
                InstanceId = activityInstance.Id,
                InstanceDate = activityInstance.ForDate,
                InstanceName = activityInstance.Name
            });
        }

        // rows of report
        var userClasses = await _activityRepo.GetUserClassesByActivityId(id);
        foreach (var userClass in userClasses)
        {
            var userAttendance = new UserAttendance
            {
                ClassId = userClass.Classroom!.Id,
                ClassName = userClass.Classroom.Name,
                UserId = userClass.User!.Id,
                UserName = userClass.User.Name,
                UserTypeId = userClass.UserTypeId,
                UserType = userClass.UserType!.Name
            };

            foreach (var activityInstance in activityInstances)
            {
                if (activityInstance.ActivityInstanceUsers.Any(a => a.UserId == userClass.UserId))
                {
                    userAttendance.InstanceIds.Add(activityInstance.Id);
                }
            }

            result.Users.Add(userAttendance);
        }

        return result;
    }
}

