namespace VModels.DTOS.ActivityInstanceUsers.Queries
{
    public class GetActivityInstanceWithUsersDto
    {
        public int ActivityId { get; set; }
        public string? ActivityName { get; set; }
        public int ActivityInstanceId { get; set; }
        public string? ActivityInstanceName { get; set; }
        public List<UserAttendanceDto> Users { get; set; } = new();// all users 10  7  3 
        // q1 => fill obj 
        // q2 => classroom => intance 
        // q3 => useractivityinstance 
    }
    public class UserAttendanceDto
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int UserTypeId { get; set; }
        public string? UserType { get; set; }
        public bool IsAttend { get; set; }
    }

}
