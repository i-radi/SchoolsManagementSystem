namespace Presentation.Controllers.MVC
{
    public class ActivityInstancesController : Controller
    {
        private readonly IActivityInstanceRepo _activityInstanceRepo;
        private readonly IActivityRepo _activityRepo;
        private readonly ISeasonRepo _seasonRepo;

        public ActivityInstancesController(
            IActivityInstanceRepo activityInstanceRepo,
            IActivityRepo activityRepo,
            ISeasonRepo seasonRepo)
        {
            _activityInstanceRepo = activityInstanceRepo;
            _activityRepo = activityRepo;
            _seasonRepo = seasonRepo;
        }

        // GET: ActivityInstances
        public async Task<IActionResult> Index(int? activityId, int? id)
        {
            var models = _activityInstanceRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .Include(a => a.Season)
                .AsQueryable();
            if (activityId is not null)
            {
                models = models.Where(a => a.ActivityId == activityId.Value);
            }
            if (id is not null)
            {
                var activityInstance = await _activityInstanceRepo.GetByIdAsync(id.Value);
                models = models.Where(a => a.ActivityId == activityInstance.ActivityId);
            }
            return View(await models.ToListAsync());
        }

        // GET: ActivityInstances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _activityInstanceRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityInstance = await _activityInstanceRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .Include(a => a.Season)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityInstance == null)
            {
                return NotFound();
            }

            return View(activityInstance);
        }

        // GET: ActivityInstances/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: ActivityInstances/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityId,SeasonId,Name,CreatedDate,ForDate,IsLocked")] ActivityInstance activityInstance)
        {
            if (ModelState.IsValid)
            {
                await _activityInstanceRepo.AddAsync(activityInstance);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstance.ActivityId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstance.SeasonId);
            return View(activityInstance);
        }

        // GET: ActivityInstances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _activityInstanceRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityInstance = await _activityInstanceRepo.GetByIdAsync(id.Value);
            if (activityInstance == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstance.ActivityId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstance.SeasonId);
            return View(activityInstance);
        }

        // POST: ActivityInstances/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityId,SeasonId,Name,CreatedDate,ForDate,IsLocked")] ActivityInstance activityInstance)
        {
            if (id != activityInstance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _activityInstanceRepo.UpdateAsync(activityInstance);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityInstanceExists(activityInstance.Id))
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
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstance.ActivityId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstance.SeasonId);
            return View(activityInstance);
        }

        // GET: ActivityInstances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _activityInstanceRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityInstance = await _activityInstanceRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .Include(a => a.Season)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityInstance == null)
            {
                return NotFound();
            }

            return View(activityInstance);
        }

        // POST: ActivityInstances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_activityInstanceRepo.GetTableNoTracking().ToList() == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ActivityInstances'  is null.");
            }
            var activityInstance = await _activityInstanceRepo.GetByIdAsync(id);
            if (activityInstance != null)
            {
                await _activityInstanceRepo.DeleteAsync(activityInstance);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActivityInstanceExists(int id)
        {
            return (_activityInstanceRepo.GetTableNoTracking()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
