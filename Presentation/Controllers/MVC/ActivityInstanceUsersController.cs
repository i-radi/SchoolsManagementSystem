namespace Presentation.Controllers.MVC
{
    public class ActivityInstanceUsersController : Controller
    {
        private readonly IActivityInstanceUserRepo _activityInstanceUserRepo;
        private readonly IActivityInstanceRepo _activityInstanceRepo;
        private readonly ApplicationDBContext _context;
        private readonly IMapper _mapper;

        public ActivityInstanceUsersController(
            IActivityInstanceUserRepo activityInstanceUserRepo,
            IActivityInstanceRepo activityInstanceRepo,
            ApplicationDBContext context,
            IMapper mapper)
        {
            _activityInstanceUserRepo = activityInstanceUserRepo;
            _activityInstanceRepo = activityInstanceRepo;
            _context = context;
            _mapper = mapper;
        }

        // GET: ActivityInstanceUsers
        public async Task<IActionResult> Index(int? instanceId)
        {
            var models = _context.ActivityInstanceUsers.Include(a => a.ActivityInstance).Include(a => a.User).AsQueryable();
            if (instanceId is not null)
            {
                models = models
                    .Where(u => u.ActivityInstanceId == instanceId);
            }
            var viewmodels = _mapper.Map<List<ActivityInstanceUserViewModel>>(await models.ToListAsync());
            ViewBag.InstanceId = instanceId;
            return View(viewmodels);
        }

        // GET: ActivityInstanceUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ActivityInstanceUsers == null)
            {
                return NotFound();
            }

            var activityInstanceUser = await _context.ActivityInstanceUsers
                .Include(a => a.ActivityInstance)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityInstanceUser == null)
            {
                return NotFound();
            }
            var activityInstanceUserVM = _mapper.Map<ActivityInstanceUserViewModel>(activityInstanceUser);
            return View(activityInstanceUserVM);
        }

        // GET: ActivityInstanceUsers/Create
        public async Task<IActionResult> Create(int instanceId)
        {
            var activityInstance = await _context.ActivityInstances
                .Where(a => a.Id == instanceId)
                .ToListAsync();
            ViewData["ActivityInstanceId"] = new SelectList(activityInstance, "Id", "Name");

            var currentUserIds = await _activityInstanceUserRepo
                .GetTableNoTracking()
                .Where(a => a.ActivityInstanceId == instanceId)
                .Select(a => a.UserId).ToListAsync();
            var orgId = await _context.ActivityInstances
                .Where(a => a.Id == instanceId)
                .Include(a => a.Activity)
                .ThenInclude(a => a!.School)
                .Select(a => a.Activity!.School!.OrganizationId)
                .FirstOrDefaultAsync();

            var allowedUsers = await _context.User
                .Include(u => u.UserClasses)
                .Where(u => (!currentUserIds.Contains(u.Id)))
                .ToListAsync();
            ViewData["UserId"] = new SelectList(allowedUsers, "Id", "Name");
            return View(new ActivityInstanceUserViewModel());
        }

        // POST: ActivityInstanceUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityInstanceUserViewModel activityInstanceUserVM)
        {
            if (ModelState.IsValid)
            {
                var activityInstanceUser = _mapper.Map<ActivityInstanceUser>(activityInstanceUserVM);
                _context.Add(activityInstanceUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { instanceId = activityInstanceUserVM.ActivityInstanceId });
            }
            ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name", activityInstanceUserVM.ActivityInstanceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", activityInstanceUserVM.UserId);
            return View(activityInstanceUserVM);
        }

        // GET: ActivityInstanceUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ActivityInstanceUsers == null)
            {
                return NotFound();
            }

            var activityInstanceUser = await _context.ActivityInstanceUsers.FindAsync(id);
            if (activityInstanceUser == null)
            {
                return NotFound();
            }
            ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name", activityInstanceUser.ActivityInstanceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", activityInstanceUser.UserId);
            var activityInstanceUserVM = _mapper.Map<ActivityInstanceUserViewModel>(activityInstanceUser);
            return View(activityInstanceUserVM);
        }

        // POST: ActivityInstanceUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ActivityInstanceUserViewModel activityInstanceUserVM)
        {
            if (id != activityInstanceUserVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var activityInstanceUser = _mapper.Map<ActivityInstanceUser>(activityInstanceUserVM);
                    _context.Update(activityInstanceUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityInstanceUserExists(activityInstanceUserVM.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { instanceId = activityInstanceUserVM.ActivityInstanceId });
            }
            ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name", activityInstanceUserVM.ActivityInstanceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", activityInstanceUserVM.UserId);
            return View(activityInstanceUserVM);
        }

        // GET: ActivityInstanceUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActivityInstanceUsers == null)
            {
                return NotFound();
            }

            var activityInstanceUser = await _context.ActivityInstanceUsers
                .Include(a => a.ActivityInstance)
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityInstanceUser == null)
            {
                return NotFound();
            }

            var activityInstanceUserVM = _mapper.Map<ActivityInstanceUserViewModel>(activityInstanceUser);
            return View(activityInstanceUserVM);
        }

        // POST: ActivityInstanceUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActivityInstanceUsers == null)
            {
                return Problem("Entity set 'ApplicationDBContext.ActivityInstanceUsers'  is null.");
            }
            var activityInstanceUser = await _context.ActivityInstanceUsers.FindAsync(id);
            if (activityInstanceUser != null)
            {
                _context.ActivityInstanceUsers.Remove(activityInstanceUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { instanceId = activityInstanceUser.ActivityInstanceId });
        }

        private bool ActivityInstanceUserExists(int id)
        {
            return (_context.ActivityInstanceUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
