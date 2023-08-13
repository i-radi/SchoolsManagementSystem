using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;

namespace Presentation.Controllers.MVC
{
    public class ActivitiesController : Controller
    {
        private readonly IActivityRepo _activityRepo;
        private readonly ISchoolRepo _schoolRepo;

        public ActivitiesController(IActivityRepo activityRepo, ISchoolRepo schoolRepo)
        {
            _activityRepo = activityRepo;
            _schoolRepo = schoolRepo;
        }

        // GET: Activities
        public async Task<IActionResult> Index(int schoolId)
        {
            var models = _activityRepo.GetTableNoTracking().Include(c => c.School).AsQueryable();
            if (schoolId > 0)
            {
                models = models.Where(c => c.SchoolId == schoolId);
            }

            return View(await models.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _activityRepo.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableAsTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SchoolId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                var schoolName  = (await _schoolRepo.GetByIdAsync(activity.SchoolId)).Name;
                activity.Role = new Role
                {
                    Name = (activity.Title + "@" + schoolName).Replace(" ", String.Empty),
                    NormalizedName = (activity.Title + "@" + schoolName).Replace(" ", String.Empty).ToUpper()
                };
                await _activityRepo.AddAsync(activity);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableAsTracking().ToList(), "Id", "Name", activity.SchoolId);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _activityRepo.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableAsTracking().ToList(), "Id", "Name", activity.SchoolId);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,SchoolId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    activity.RoleId =  _activityRepo.GetTableNoTracking().FirstOrDefault(a => a.Id == activity.Id)!.RoleId;
                    await _activityRepo.UpdateAsync(activity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableAsTracking().ToList(), "Id", "Name", activity.SchoolId);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _activityRepo.GetByIdAsync(id.Value);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity != null)
            {
                await _activityRepo.DeleteAsync(activity);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return (_activityRepo.GetTableNoTracking().ToList().Any(e => e.Id == id));
        }
    }
}
