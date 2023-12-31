﻿using VModels.ViewModels.Attendances;

namespace Core.IServices;

public interface IAttendanceService
{
    Task<ActivityAttendanceViewModel?> GetByActivityId(int id);
    Task<RecordAttendanceViewModel?> GetByRecordId(int id);
}
