namespace Presentation.Controllers.MVC
{
    public class UserRecordsController(
        IUserRecordRepo userRecordRepo,
        IRecordRepo recordRepo,
        UserManager<User> userManager,
        IOrganizationRepo organizationRepo,
        IMapper mapper) : Controller
    {
        private readonly IUserRecordRepo _userRecordRepo = userRecordRepo;
        private readonly IRecordRepo _recordRepo = recordRepo;
        private readonly UserManager<User> _userManager = userManager;
        private readonly IOrganizationRepo _organizationRepo = organizationRepo;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index(string searchUserName = "", int userId = 0)
        {
            var userRecords = _userRecordRepo
                .GetTableNoTracking()
                .Include(g => g.Record)
                .Include(g => g.User)
                .AsQueryable();

            if (userId > 0)
            {
                userRecords = userRecords.Where(g => g.UserId == userId);
            }

            if (!string.IsNullOrWhiteSpace(searchUserName))
            {
                userRecords = userRecords.Where(uc => uc.User!.Email!.ToLower().Contains(searchUserName.ToLower())
                || uc.User!.Name!.ToLower().Contains(searchUserName.ToLower()));
            }

            var userRecordsVM = _mapper.Map<List<UserRecordViewModel>>(await userRecords.ToListAsync());
            return View(userRecordsVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRecord = await _userRecordRepo.GetByIdAsync(id.Value);
            if (userRecord == null)
            {
                return NotFound();
            }

            var userRecordVM = _mapper.Map<UserRecordViewModel>(userRecord);
            return View(userRecordVM);
        }

        public IActionResult Create(int userId = 0)
        {
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available).ToList(), "Id", "Name");
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            if (userId > 0)
            {
                ViewData["UserId"] = new SelectList(_userManager.Users.Where(u => u.Id == userId).ToList(), "Id", "Name");

            }
            else
            {
                ViewData["UserId"] = new SelectList(_userManager.Users.ToList(), "Id", "Name");
            }
            return View(new UserRecordViewModel { UserId = userId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRecordViewModel userRecordVM)
        {
            if (ModelState.IsValid)
            {
                var userRecord = _mapper.Map<UserRecord>(userRecordVM);

                await _userRecordRepo.AddAsync(userRecord);
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available).ToList(), "Id", "Name", userRecordVM.RecordId);
            return View(userRecordVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRecord = await _userRecordRepo.GetByIdAsync(id.Value);
            if (userRecord == null)
            {
                return NotFound();
            }
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available).ToList(), "Id", "Name", userRecord.RecordId);
            ViewData["UserId"] = new SelectList(_userManager.Users.ToList(), "Id", "Name");
            var userRecordVM = _mapper.Map<UserRecordViewModel>(userRecord);
            return View(userRecordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserRecordViewModel userRecordVM)
        {
            if (id != userRecordVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userRecord = _mapper.Map<UserRecord>(userRecordVM);
                    await _userRecordRepo.UpdateAsync(userRecord);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRecordExists(userRecordVM.Id))
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
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available).ToList(), "Id", "Name", userRecordVM.RecordId);
            return View(userRecordVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRecord = await _userRecordRepo.GetByIdAsync(id.Value);
            if (userRecord == null)
            {
                return NotFound();
            }

            var userRecordVM = _mapper.Map<UserRecordViewModel>(userRecord);
            return View(userRecordVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRecord = await _userRecordRepo.GetByIdAsync((int)id);
            if (userRecord != null)
            {
                await _userRecordRepo.DeleteAsync(userRecord);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UserRecordExists(int id)
        {
            return (_userRecordRepo.GetTableNoTracking().Any(e => e.Id == id));
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersByOrganization(int organizationId)
        {
            var users = await _userManager.Users
                .Where(s => s.UserOrganizations.Any(o => o.OrganizationId == organizationId))
                .ToListAsync();

            var userList = users.Select(user => new SelectListItem
            {
                Value = user.Id.ToString(),
                Text = user.Name
            }).ToList();

            return Json(userList);
        }
    }
}
