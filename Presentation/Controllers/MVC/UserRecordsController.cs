namespace Presentation.Controllers.MVC
{
    public class UserRecordsController : Controller
    {
        private readonly IUserRecordRepo _userRecordRepo;
        private readonly IRecordRepo _recordRepo;
        private readonly IMapper _mapper;

        public UserRecordsController(
            IUserRecordRepo userRecordRepo,
            IRecordRepo recordRepo,
            IMapper mapper)
        {
            _recordRepo = recordRepo;
            _userRecordRepo = userRecordRepo;
            _mapper = mapper;
        }

        // GET: UserRecords
        public async Task<IActionResult> Index(int userId)
        {
            var userRecords = _userRecordRepo
                .GetTableNoTracking()
                .Include(g => g.Record)
                .AsQueryable();

            if (userId > 0)
            {
                userRecords = userRecords.Where(g => g.UserId == userId);
            }
            var userRecordsVM = _mapper.Map<List<UserRecordViewModel>>(await userRecords.ToListAsync());
            return View(userRecordsVM);
        }

        // GET: UserRecords/Details/5
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

        // GET: UserRecords/Create
        public IActionResult Create()
        {
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: UserRecords/Create
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
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().ToList(), "Id", "Name", userRecordVM.RecordId);
            return View(userRecordVM);
        }

        // GET: UserRecords/Edit/5
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
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().ToList(), "Id", "Name", userRecord.RecordId);
            var userRecordVM = _mapper.Map<UserRecordViewModel>(userRecord);
            return View(userRecordVM);
        }

        // POST: UserRecords/Edit/5
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
            ViewData["RecordId"] = new SelectList(_recordRepo.GetTableNoTracking().ToList(), "Id", "Name", userRecordVM.RecordId);
            return View(userRecordVM);
        }

        // GET: UserRecords/Delete/5
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

        // POST: UserRecords/Delete/5
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
    }
}
