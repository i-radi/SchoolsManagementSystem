namespace Presentation.Controllers.MVC
{
    public class ActivityTimesController : Controller
    {
        private readonly IActivityTimeRepo _activityTimeRepo;
        private readonly IActivityRepo _activityRepo;

        public ActivityTimesController(
            IActivityTimeRepo activityTimeRepo,
            IActivityRepo activityRepo)
        {
            _activityTimeRepo = activityTimeRepo;
            _activityRepo = activityRepo;
        }

        // GET: ActivityTimes
        public async Task<IActionResult> Index(int? activityId)
        {
            var models = _activityTimeRepo.GetTableNoTracking().Include(a => a.Activity).AsQueryable();
            if (activityId is not null)
            {
                models = models.Where(a => a.ActivityId == activityId.Value);
            }
            return View(await models.ToListAsync());
        }

        // GET: ActivityTimes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _activityTimeRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityTime = await _activityTimeRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityTime == null)
            {
                return NotFound();
            }

            return View(activityTime);
        }

        // GET: ActivityTimes/Create
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: ActivityTimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityId,Day,FromTime,ToTime,Body")] ActivityTime activityTime)
        {
            if (ModelState.IsValid)
            {
                await _activityTimeRepo.AddAsync(activityTime);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityTime.ActivityId);
            return View(activityTime);
        }

        // GET: ActivityTimes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _activityTimeRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityTime = await _activityTimeRepo.GetByIdAsync(id.Value);
            if (activityTime == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityTime.ActivityId);
            return View(activityTime);
        }

        // POST: ActivityTimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityId,Day,FromTime,ToTime,Body")] ActivityTime activityTime)
        {
            if (id != activityTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _activityTimeRepo.UpdateAsync(activityTime);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTimeExists(activityTime.Id))
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
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityTime.ActivityId);
            return View(activityTime);
        }

        // GET: ActivityTimes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _activityTimeRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityTime = await _activityTimeRepo.GetTableNoTracking()
                .Include(a => a.Activity)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityTime == null)
            {
                return NotFound();
            }

            return View(activityTime);
        }

        // POST: ActivityTimes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_activityTimeRepo.GetTableNoTracking().ToList() == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ActivityTimes'  is null.");
            }
            var activityTime = await _activityTimeRepo.GetByIdAsync(id);
            if (activityTime != null)
            {
                await _activityTimeRepo.DeleteAsync(activityTime);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActivityTimeExists(int id)
        {
            return (_activityTimeRepo.GetTableNoTracking()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
