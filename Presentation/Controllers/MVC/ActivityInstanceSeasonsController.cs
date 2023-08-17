using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;

namespace Presentation.Controllers.MVC
{
    public class ActivityInstanceSeasonsController : Controller
    {
        private readonly IActivityInstanceSeasonRepo _activityInstanceSeasonRepo;
        private readonly IActivityInstanceRepo _activityInstanceRepo;
        private readonly ISeasonRepo _seasonRepo;

        public ActivityInstanceSeasonsController(
            IActivityInstanceSeasonRepo activityInstanceSeasonRepo,
            IActivityInstanceRepo activityInstanceRepo,
            ISeasonRepo seasonRepo)
        {
            _activityInstanceSeasonRepo = activityInstanceSeasonRepo;
            _activityInstanceRepo = activityInstanceRepo;
            _seasonRepo = seasonRepo;
        }

        // GET: ActivityInstanceSeasons
        public async Task<IActionResult> Index(int? instanceId)
        {
            var models = _activityInstanceSeasonRepo
                .GetTableNoTracking()
                .Include(a => a.ActivityInstance)
                .Include(a => a.Season)
                .AsQueryable();

            if (instanceId is not null)
            {
                models = models.Where(models => models.ActivityInstanceId == instanceId);
            }
            return View(await models.ToListAsync());
        }

        // GET: ActivityInstanceSeasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _activityInstanceSeasonRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityInstanceSeason = await _activityInstanceSeasonRepo
                .GetTableNoTracking()
                .Include(a => a.ActivityInstance)
                .Include(a => a.Season)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityInstanceSeason == null)
            {
                return NotFound();
            }

            return View(activityInstanceSeason);
        }

        // GET: ActivityInstanceSeasons/Create
        public IActionResult Create()
        {
            ViewData["ActivityInstanceId"] = new SelectList(_activityInstanceRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "To");
            return View();
        }

        // POST: ActivityInstanceSeasons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityInstanceId,SeasonId")] ActivityInstanceSeason activityInstanceSeason)
        {
            if (ModelState.IsValid)
            {
                await _activityInstanceSeasonRepo.AddAsync(activityInstanceSeason);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityInstanceId"] = new SelectList(_activityInstanceRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceSeason.ActivityInstanceId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "To", activityInstanceSeason.SeasonId);
            return View(activityInstanceSeason);
        }

        // GET: ActivityInstanceSeasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _activityInstanceSeasonRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityInstanceSeason = await _activityInstanceSeasonRepo.GetByIdAsync(id.Value);
            if (activityInstanceSeason == null)
            {
                return NotFound();
            }
            ViewData["ActivityInstanceId"] = new SelectList(_activityInstanceRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceSeason.ActivityInstanceId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "To", activityInstanceSeason.SeasonId);
            return View(activityInstanceSeason);
        }

        // POST: ActivityInstanceSeasons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityInstanceId,SeasonId")] ActivityInstanceSeason activityInstanceSeason)
        {
            if (id != activityInstanceSeason.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _activityInstanceSeasonRepo.UpdateAsync(activityInstanceSeason);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityInstanceSeasonExists(activityInstanceSeason.Id))
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
            ViewData["ActivityInstanceId"] = new SelectList(_activityInstanceRepo.GetTableNoTracking().ToList(), "Id", "Name", activityInstanceSeason.ActivityInstanceId);
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking().ToList(), "Id", "To", activityInstanceSeason.SeasonId);
            return View(activityInstanceSeason);
        }

        // GET: ActivityInstanceSeasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _activityInstanceSeasonRepo.GetTableNoTracking().ToList() == null)
            {
                return NotFound();
            }

            var activityInstanceSeason = await _activityInstanceSeasonRepo
                .GetTableNoTracking()
                .Include(a => a.ActivityInstance)
                .Include(a => a.Season)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityInstanceSeason == null)
            {
                return NotFound();
            }

            return View(activityInstanceSeason);
        }

        // POST: ActivityInstanceSeasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_activityInstanceSeasonRepo.GetTableNoTracking().ToList() == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ActivityInstanceSeasons'  is null.");
            }
            var activityInstanceSeason = await _activityInstanceSeasonRepo.GetByIdAsync(id);
            if (activityInstanceSeason != null)
            {
                await _activityInstanceSeasonRepo.DeleteAsync(activityInstanceSeason);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActivityInstanceSeasonExists(int id)
        {
            return (_activityInstanceSeasonRepo.GetTableNoTracking()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
