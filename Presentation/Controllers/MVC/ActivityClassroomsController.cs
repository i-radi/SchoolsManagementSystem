namespace Presentation.Controllers.MVC
{
    public class ActivityClassroomsController(
        IActivityClassroomRepo activityClassroomRepo,
        IActivityRepo activityRepo,
        IClassroomRepo classroomRepo,
        IMapper mapper) : Controller
    {
        private readonly IActivityClassroomRepo _activityClassroomRepo = activityClassroomRepo;
        private readonly IActivityRepo _activityRepo = activityRepo;
        private readonly IClassroomRepo _classroomRepo = classroomRepo;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index(int? activityId)
        {
            var models = _activityClassroomRepo.GetTableNoTracking().Include(a => a.Activity).Include(a => a.Classroom).AsQueryable();
            if (activityId is not null)
            {
                models = models.Where(a => a.ActivityId == activityId.Value);
            }

            var viewmodels = _mapper.Map<List<ActivityClassroomViewModel>>(await models.ToListAsync());
            return View(viewmodels);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _activityClassroomRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityClassroom = await _activityClassroomRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .Include(a => a.Classroom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityClassroom == null)
            {
                return NotFound();
            }
            var activityClassroomVM = _mapper.Map<ActivityClassroomViewModel>(activityClassroom);
            return View(activityClassroomVM);
        }

        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["ClassroomId"] = new SelectList(_classroomRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityClassroomViewModel activityClassroomVM)
        {
            if (ModelState.IsValid)
            {
                var activityClassroom = _mapper.Map<ActivityClassroom>(activityClassroomVM);
                await _activityClassroomRepo.AddAsync(activityClassroom);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityClassroomVM.ActivityId);
            ViewData["ClassroomId"] = new SelectList(_classroomRepo.GetTableNoTracking().ToList(), "Id", "Name", activityClassroomVM.ClassroomId);
            return View(activityClassroomVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _activityClassroomRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityClassroom = await _activityClassroomRepo.GetByIdAsync(id.Value);
            if (activityClassroom == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityClassroom.ActivityId);
            ViewData["ClassroomId"] = new SelectList(_classroomRepo.GetTableNoTracking().ToList(), "Id", "Name", activityClassroom.ClassroomId);
            var activityClassroomVM = _mapper.Map<ActivityClassroomViewModel>(activityClassroom);
            return View(activityClassroomVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityClassroomViewModel activityClassroomVM)
        {
            if (id != activityClassroomVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activityClassroom = _mapper.Map<ActivityClassroom>(activityClassroomVM);

                    await _activityClassroomRepo.AddAsync(activityClassroom);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityClassroomExists(activityClassroomVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityClassroomVM.ActivityId);
            ViewData["ClassroomId"] = new SelectList(_classroomRepo.GetTableNoTracking().ToList(), "Id", "Name", activityClassroomVM.ClassroomId);
            return View(activityClassroomVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _activityClassroomRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityClassroom = await _activityClassroomRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .Include(a => a.Classroom)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityClassroom == null)
            {
                return NotFound();
            }
            var activityClassroomVM = _mapper.Map<ActivityClassroomViewModel>(activityClassroom);

            return View(activityClassroomVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_activityClassroomRepo.GetTableNoTracking().ToList() == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ActivityClassrooms'  is null.");
            }
            var activityClassroom = await _activityClassroomRepo.GetByIdAsync(id);
            if (activityClassroom != null)
            {
                await _activityClassroomRepo.DeleteAsync(activityClassroom);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActivityClassroomExists(int id)
        {
            return (_activityClassroomRepo.GetTableNoTracking()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
