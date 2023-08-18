namespace Presentation.Controllers.MVC
{
    public class ActivityInstanceUsersController : Controller
    {
        private readonly IActivityInstanceUserRepo _activityInstanceUserRepo;
        private readonly IActivityInstanceRepo _activityInstanceRepo;
        private readonly ApplicationDBContext _context;

        public ActivityInstanceUsersController(
            IActivityInstanceUserRepo activityInstanceUserRepo,
            IActivityInstanceRepo activityInstanceRepo,
            ApplicationDBContext context)
        {
            _activityInstanceUserRepo = activityInstanceUserRepo;
            _activityInstanceRepo = activityInstanceRepo;
            _context = context;
        }

        // GET: ActivityInstanceUsers
        public async Task<IActionResult> Index(int? instanceId)
        {
            var applicationDBContext = _context.ActivityInstanceUsers.Include(a => a.ActivityInstance).Include(a => a.User).AsQueryable();
            if (instanceId is not null)
            {
                applicationDBContext = applicationDBContext
                    .Where(u => u.ActivityInstanceId == instanceId);
            }
            return View(await applicationDBContext.ToListAsync());
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

            return View(activityInstanceUser);
        }

        // GET: ActivityInstanceUsers/Create
        public IActionResult Create()
        {
            ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name");
            return View(new ActivityInstanceUser());
        }

        // POST: ActivityInstanceUsers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ActivityInstanceId,UserId,Note,CreatedDate")] ActivityInstanceUser activityInstanceUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityInstanceUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name", activityInstanceUser.ActivityInstanceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", activityInstanceUser.UserId);
            return View(activityInstanceUser);
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
            return View(activityInstanceUser);
        }

        // POST: ActivityInstanceUsers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ActivityInstanceId,UserId,Note,CreatedDate")] ActivityInstanceUser activityInstanceUser)
        {
            if (id != activityInstanceUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activityInstanceUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityInstanceUserExists(activityInstanceUser.Id))
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
            ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name", activityInstanceUser.ActivityInstanceId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", activityInstanceUser.UserId);
            return View(activityInstanceUser);
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

            return View(activityInstanceUser);
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
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityInstanceUserExists(int id)
        {
            return (_context.ActivityInstanceUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
