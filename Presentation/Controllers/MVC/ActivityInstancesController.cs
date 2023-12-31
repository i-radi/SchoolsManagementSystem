﻿namespace Presentation.Controllers.MVC
{
    public class ActivityInstancesController(
        IActivityInstanceRepo activityInstanceRepo,
        IActivityRepo activityRepo,
        ISeasonRepo seasonRepo,
        IMapper mapper) : Controller
    {
        private readonly IActivityInstanceRepo _activityInstanceRepo = activityInstanceRepo;
        private readonly IActivityRepo _activityRepo = activityRepo;
        private readonly ISeasonRepo _seasonRepo = seasonRepo;
        private readonly IMapper _mapper = mapper;

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
            var viewmodels = _mapper.Map<List<ActivityInstanceViewModel>>(await models.ToListAsync());
            return View(viewmodels);
        }

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

            var activityInstanceVM = _mapper.Map<ActivityInstanceViewModel>(activityInstance);
            return View(activityInstanceVM);
        }

        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityInstanceViewModel activityInstanceVM)
        {
            if (ModelState.IsValid)
            {
                var activityInstance = _mapper.Map<ActivityInstance>(activityInstanceVM);
                await _activityInstanceRepo.AddAsync(activityInstance);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceVM.ActivityId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceVM.SeasonId);
            return View(activityInstanceVM);
        }

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
            var activityInstanceVM = _mapper.Map<ActivityInstanceViewModel>(activityInstance);
            return View(activityInstanceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityInstanceViewModel activityInstanceVM)
        {
            if (id != activityInstanceVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activityInstance = _mapper.Map<ActivityInstance>(activityInstanceVM);
                    await _activityInstanceRepo.UpdateAsync(activityInstance);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityInstanceExists(activityInstanceVM.Id))
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
            ViewData["ActivityId"] = new SelectList(_activityRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceVM.ActivityId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceVM.SeasonId);
            return View(activityInstanceVM);
        }

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

            var activityInstanceVM = _mapper.Map<ActivityInstanceViewModel>(activityInstance);
            return View(activityInstanceVM);
        }

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
