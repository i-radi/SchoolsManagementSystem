namespace Presentation.Controllers.MVC
{
    public class ActivitiesController : Controller
    {
        private readonly IActivityRepo _activityRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IMapper _mapper;

        public ActivitiesController(
            IActivityRepo activityRepo,
            ISchoolRepo schoolRepo,
            IMapper mapper)
        {
            _activityRepo = activityRepo;
            _schoolRepo = schoolRepo;
            _mapper = mapper;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var models = await _activityRepo.GetTableNoTracking().Include(a => a.School).ToListAsync();
            var viewmodels = _mapper.Map<List<ActivityViewModel>>(models);
            return View(viewmodels);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _activityRepo.GetTableNoTracking() == null)
            {
                return NotFound();
            }

            var activity = await _activityRepo.GetTableNoTracking()
                .Include(a => a.School)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }
            var viewmodel = _mapper.Map<ActivityViewModel>(activity);

            return View(viewmodel);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel activityVM)
        {
            if (ModelState.IsValid)
            {
                var activity = _mapper.Map<Activity>(activityVM);
                await _activityRepo.AddAsync(activity);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", activityVM.SchoolId);
            return View(activityVM);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _activityRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activity = await _activityRepo.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", activity.SchoolId);
            var activityVM = _mapper.Map<ActivityViewModel>(activity);
            return View(activityVM);
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityViewModel activityVM)
        {
            if (id != activityVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                var activity = _mapper.Map<Activity>(activityVM);
                    await _activityRepo.UpdateAsync(activity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activityVM.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", activityVM.SchoolId);
            return View(activityVM);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _schoolRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activity = await _activityRepo.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            var viewmodel = _mapper.Map<ActivityViewModel>(activity);
            return View(viewmodel);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_activityRepo.GetTableNoTracking().ToList() == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Activities'  is null.");
            }
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity != null)
            {
                await _activityRepo.DeleteAsync(activity);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return (_activityRepo.GetTableNoTracking()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
