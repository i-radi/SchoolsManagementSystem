using VModels.ViewModels;

namespace Presentation.Controllers.MVC
{
    public class ActivityInstanceUsersController(
        IActivityInstanceUserRepo activityInstanceUserRepo,
        IActivityInstanceRepo activityInstanceRepo,
        ApplicationDBContext context,
        IMapper mapper) : Controller
    {
        private readonly IActivityInstanceUserRepo _activityInstanceUserRepo = activityInstanceUserRepo;
        private readonly IActivityInstanceRepo _activityInstanceRepo = activityInstanceRepo;
        private readonly ApplicationDBContext _context = context;
        private readonly IMapper _mapper = mapper;

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

        //public async Task<IActionResult> Create(int instanceId)
        //{
        //    var activityInstance = await _context.ActivityInstances
        //        .Where(a => a.Id == instanceId)
        //        .ToListAsync();
        //    ViewData["ActivityInstanceId"] = new SelectList(activityInstance, "Id", "Name");



        //    var currentUserIds = await _activityInstanceUserRepo
        //        .GetTableNoTracking()
        //        .Where(a => a.ActivityInstanceId == instanceId)
        //        .Select(a => a.UserId).ToListAsync();
        //    var orgId = await _context.ActivityInstances
        //        .Where(a => a.Id == instanceId)
        //        .Include(a => a.Activity)
        //        .ThenInclude(a => a!.School)
        //        .Select(a => a.Activity!.School!.OrganizationId)
        //        .FirstOrDefaultAsync();

        //    var allowedUsers = await _context.User
        //        .Include(u => u.UserClasses)
        //        .Where(u => (!currentUserIds.Contains(u.Id))
        //        && u.UserOrganizations.Any())
        //        .ToListAsync();
        //    ViewData["UserId"] = new SelectList(allowedUsers, "Id", "Name");
        //    return View(new ActivityInstanceUserViewModel());
        //}
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
                .Where(u => (!currentUserIds.Contains(u.Id))
                && u.UserOrganizations.Any())
                .ToListAsync();
            ActivityInstanceUserDataViewModel activityInstanceUserData = new ActivityInstanceUserDataViewModel();
            foreach (var item in allowedUsers)
            {

                activityInstanceUserData.activityInstanceUsersDataViewModels
                 .Add(new ActivityInstanceUsersDataViewMode() { UserId = item.Id , UserName =item.UserName});

                                                                                                                                                                                                                                                                                                            
            }
            return View(activityInstanceUserData);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(ActivityInstanceUserViewModel activityInstanceUserVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // foreach in userIds => insId userid note 
        //        var activityInstanceUser = _mapper.Map<ActivityInstanceUser>(activityInstanceUserVM);
        //        _context.Add(activityInstanceUser);
        //        await _context.SaveChangesAsync();  
        //        return RedirectToAction(nameof(Index), new { instanceId = activityInstanceUserVM.ActivityInstanceId });
        //    }
        //    ViewData["ActivityInstanceId"] = new SelectList(_context.ActivityInstances, "Id", "Name", activityInstanceUserVM.ActivityInstanceId);
        //    ViewData["UserId"] = new SelectList(_context.User, "Id", "Name", activityInstanceUserVM.UserId);
        //    return View(activityInstanceUserVM);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityInstanceUserDataViewModel activityInstanceUserVM)
        {
                     
                List<ActivityInstanceUser> activityInstanceUsers = new List<ActivityInstanceUser>();
                  if(activityInstanceUserVM.IsSelectAll)
                    {
                        foreach (var item in activityInstanceUserVM.activityInstanceUsersDataViewModels)
                        {
                            item.IsSelected = true;
                        }

                    }  
            foreach (var item in activityInstanceUserVM.activityInstanceUsersDataViewModels)
                {
                    if(item.IsSelected)
                    {
                        activityInstanceUsers.Add(new ActivityInstanceUser()
                        { 
                             CreatedDate = DateTime.Now, 
                             ActivityInstanceId = activityInstanceUserVM.ActivityInstanceId , 
                             Note = item.Note , 
                             UserId = item.UserId 
                              
                        });
                    };
                }
                _context.AddRange(activityInstanceUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { instanceId = activityInstanceUserVM.ActivityInstanceId });
            
        }
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
