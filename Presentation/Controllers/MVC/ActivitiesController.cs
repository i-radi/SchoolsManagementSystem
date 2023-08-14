using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;
using VModels.ViewModels.Activities;
using VModels.ViewModels.Users;

namespace Presentation.Controllers.MVC
{
    public class ActivitiesController : Controller
    {
        private readonly IActivityRepo _activityRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDBContext _dBContext;

        public ActivitiesController(
            IActivityRepo activityRepo,
            ISchoolRepo schoolRepo,
            RoleManager<Role> roleManager,
            ApplicationDBContext dBContext)
        {
            _activityRepo = activityRepo;
            _schoolRepo = schoolRepo;
            _roleManager = roleManager;
            _dBContext = dBContext;
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

        //// GET: Activities/Roles/5
        //public async Task<IActionResult> Roles(int id)
        //{
        //    var roles = new List<string>();
        //    var activity = await _activityRepo.GetByIdAsync(id);

        //    if (activity is not null)
        //    {
        //        roles = await _dBContext.Roles.Where(r => r.ActivityId == id).Select(r => r.Name).ToListAsync();
        //    }

        //    ViewBag.ActivityId = id;
        //    return View(roles);
        //}

        //// GET: Activities/DeleteRole
        //public async Task<IActionResult> DeleteRole(int activityId, string roleName)
        //{

        //    var activity = await _activityRepo.GetByIdAsync(activityId);
        //    if (activity == null)
        //    {
        //        return NotFound();
        //    }
        //    var role = await _dBContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        //    var result = await _roleManager.DeleteAsync(role!);

        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Roles", new { id = activityId });
        //    }
        //    return NotFound();
        //}

        //// GET: Activities/CreateRole/
        //public async Task<IActionResult> CreateRole(int activityId)
        //{
        //    var activity = await _activityRepo.GetByIdAsync(activityId);
        //    var viewModel = new CreateActivityRoleViewModel
        //    {
        //        ActivityTitle = activity.Title,
        //        ActivityId = activity.Id ,
        //        SchoolId = activity.SchoolId,
        //        OrganizationId = activity.School!.OrganizationId

        //    };

        //    return View(viewModel);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateRole(CreateActivityRoleViewModel viewModel)
        //{
        //    var activity = await _activityRepo.GetByIdAsync(viewModel.ActivityId);
        //    if (activity is not null)
        //    {
        //        var result = await _roleManager.CreateAsync(new Role()
        //        {
        //            Name = viewModel.RoleName,
        //            OrganizationId= viewModel.OrganizationId,
        //            SchoolId= viewModel.SchoolId,
        //            ActivityId= activity.Id ,
        //        });

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Roles", new { id = viewModel.ActivityId });
        //        }
        //    }
        //    return BadRequest();
        //}
    }
}
