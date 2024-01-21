using Swashbuckle.AspNetCore.Annotations;

namespace Presentation.Controllers.API
{
    [Route("api/attendance")]
    [ApiController]
    [Authorize]
    public class AttendanceController(IAttendanceService attendanceService,
        IActivityInstanceUserService activityInstanceUserService,
        UserManager<User> userManager,
        IActivityInstanceService activityInstanceService) : ControllerBase
    {
        private readonly IAttendanceService _attendanceService = attendanceService;
        private readonly IActivityInstanceUserService _activityInstanceUserService = activityInstanceUserService;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IActivityInstanceService _activityInstanceService = activityInstanceService;

        [ApiExplorerSettings(GroupName = "V2")]
        [SwaggerOperation(Tags = new[] { "Attendance" })]
        [HttpGet("{activityInstanceId}")]
        public async Task<IActionResult> GetByActivityInstance(int activityInstanceId)
        {
            var result = await _attendanceService.GetByActivityInstanceId(activityInstanceId);
            return Ok(result);

        }

        [ApiExplorerSettings(GroupName = "V2")]
        [SwaggerOperation(Tags = new[] { "Attendance" })]
        [HttpPost("user")]
        public async Task<IActionResult> Add(AddActivityInstanceUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
            var activityIntance = _activityInstanceService.GetById(dto.ActivityInstanceId);
            if (activityIntance == null) return BadRequest(ResultHandler.BadRequest<string>("Activity Instance  Is Not Exist"));

            return Ok(await _activityInstanceUserService.Add(dto));
        }

        [ApiExplorerSettings(GroupName = "V2")]
        [SwaggerOperation(Tags = new[] { "Attendance" })]
        [HttpPut("user/{activityinstanceuserid}")]
        public async Task<IActionResult> Update(int id, UpdateActivityInstanceUserDto dto)
        {
            if (id != dto.Id) return BadRequest(ResultHandler.BadRequest<string>("Id not matched"));

            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
            var activityIntance = _activityInstanceService.GetById(dto.ActivityInstanceId);
            if (activityIntance == null) return BadRequest(ResultHandler.BadRequest<string>("Activity Instance  Is Not Exist"));

            var result = await _activityInstanceUserService.Update(dto);
            if (!result.Data)
            {
                return BadRequest(ResultHandler.BadRequest<string>("Not Updated"));
            }

            return Ok(result);
        }

        [ApiExplorerSettings(GroupName = "V2")]
        [SwaggerOperation(Tags = new[] { "Attendance" })]
        [HttpDelete("user")]
        public async Task<IActionResult> Remove(int activityinstanceid, int userid)
        {
            var user = await _userManager.FindByIdAsync(userid.ToString());
            if (user == null) return BadRequest(ResultHandler.BadRequest<string>("User Is Not Exist"));
            var activityIntance = _activityInstanceService.GetById(activityinstanceid);
            if (activityIntance == null) return BadRequest(ResultHandler.BadRequest<string>("Activity Instance  Is Not Exist"));

            var result = await _activityInstanceUserService.Delete(userid, activityinstanceid);
            if (!result.Data)
            {
                return BadRequest(ResultHandler.BadRequest<string>("Not Deleted"));
            }
            return Ok(result);
        }
    }
}
