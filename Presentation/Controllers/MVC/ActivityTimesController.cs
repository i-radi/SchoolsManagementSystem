using Models.Entities;

namespace Presentation.Controllers.MVC
{
    public class ActivityTimesController : Controller
    {
        private readonly IActivityTimeRepo _activityTimeRepo;
        private readonly IActivityRepo _activityRepo;
        private readonly IMapper _mapper;

        public ActivityTimesController(
            IActivityTimeRepo activityTimeRepo,
            IActivityRepo activityRepo,
            IMapper mapper)
        {
            _activityTimeRepo = activityTimeRepo;
            _activityRepo = activityRepo;
            _mapper = mapper;
        }

        // GET: ActivityTimes
        public async Task<IActionResult> Index(int? activityId)
        {
            var models = _activityTimeRepo.GetTableNoTracking().Include(a => a.Activity).AsQueryable();
            if (activityId is not null)
            {
                models = models.Where(a => a.ActivityId == activityId.Value);
            }
            var viewmodels = _mapper.Map<List<ActivityTimeViewModel>>(await models.ToListAsync());
            return View(viewmodels);
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

            var activityTimeVM = _mapper.Map<ActivityTimeViewModel>(activityTime);
            return View(activityTimeVM);
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
        public async Task<IActionResult> Create(ActivityTimeViewModel activityTimeVM)
        {
            if (ModelState.IsValid)
            {
                var activityTime = _mapper.Map<ActivityTime>(activityTimeVM);
                await _activityTimeRepo.AddAsync(activityTime);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityTimeVM.ActivityId);
            return View(activityTimeVM);
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
            var activityTimeVM = _mapper.Map<ActivityTime>(activityTime);
            return View(activityTimeVM);
        }

        // POST: ActivityTimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ActivityTimeViewModel activityTimeVM)
        {
            if (id != activityTimeVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activityTime = _mapper.Map<ActivityTime>(activityTimeVM);
                    await _activityTimeRepo.UpdateAsync(activityTime);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityTimeExists(activityTimeVM.Id))
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
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityTimeVM.ActivityId);
            return View(activityTimeVM);
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

            var activityTimeVM = _mapper.Map<ActivityTimeViewModel>(activityTime); 
            return View(activityTimeVM);
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
