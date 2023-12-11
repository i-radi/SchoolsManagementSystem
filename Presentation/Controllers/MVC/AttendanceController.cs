namespace Presentation.Controllers.MVC
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        public async Task<IActionResult> Index(int activityId = 0)
        {
            if (activityId <= 0)
            {
                return RedirectToAction("Index", nameof(ActivitiesController));
            }

            var viewmodel = await _attendanceService.GetByActivityId(activityId);

            if (viewmodel is null)
            {
                return RedirectToAction("Index", nameof(ActivitiesController));
            }

            return View(viewmodel);
        }
    }
}
