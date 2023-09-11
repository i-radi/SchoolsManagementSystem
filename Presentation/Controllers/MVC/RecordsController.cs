using Microsoft.AspNetCore.Identity;

namespace Presentation.Controllers.MVC
{
    public class RecordsController : Controller
    {
        private readonly IRecordRepo _recordRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IRecordClassRepo _recordClassRepo;
        private readonly IClassroomRepo _classroomRepo;
        private readonly IOrganizationRepo _organizationRepo;
        private readonly IMapper _mapper;

        public RecordsController(
            IRecordRepo recordRepo,
            ISchoolRepo schoolRepo,
            IRecordClassRepo recordClassRepo,
            IClassroomRepo classroomRepo,
            IOrganizationRepo organizationRepo,
            IMapper mapper)
        {
            _recordRepo = recordRepo;
            _schoolRepo = schoolRepo;
            _recordClassRepo = recordClassRepo;
            _classroomRepo = classroomRepo;
            _organizationRepo = organizationRepo;
            _mapper = mapper;
        }

        // GET: Records
        public async Task<IActionResult> Index(int schoolId,int classroomId)
        {
            var records = _recordRepo
                .GetTableNoTracking()
                .Include(g => g.School)
                .Include(r => r.RecordClasses)
                .Where(r => r.Available)
                .AsQueryable();

            if (schoolId > 0)
            {
                records = records.Where(g => g.SchoolId == schoolId);
            }
            if (classroomId > 0)
            {
                records = records.Where(g => g.RecordClasses.Any(r => r.ClassroomId == classroomId));
            }
            var recordsVM = _mapper.Map<List<RecordViewModel>>(await records.ToListAsync());
            ViewBag.SchoolsList = new SelectList(_schoolRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.ClassroomsList = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            return View(recordsVM);
        }

        // GET: Records/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _recordRepo.GetByIdAsync(id.Value);
            if (record == null)
            {
                return NotFound();
            }

            var recordVM = _mapper.Map<RecordViewModel>(record);
            return View(recordVM);
        }

        // GET: Records/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Records/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecordViewModel recordVM)
        {
            if (ModelState.IsValid)
            {
                recordVM.Available = true;
                var record = _mapper.Map<Record>(recordVM);

                await _recordRepo.AddAsync(record);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", recordVM.SchoolId);
            return View(recordVM);
        }

        // GET: Records/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _recordRepo.GetByIdAsync(id.Value);
            if (record == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", record.SchoolId);
            var recordVM = _mapper.Map<RecordViewModel>(record);
            return View(recordVM);
        }

        // POST: Records/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecordViewModel recordVM)
        {
            if (id != recordVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var record = _mapper.Map<Record>(recordVM);
                    await _recordRepo.UpdateAsync(record);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecordExists(recordVM.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", recordVM.SchoolId);
            return View(recordVM);
        }

        // GET: Records/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var record = await _recordRepo.GetByIdAsync(id.Value);
            if (record == null)
            {
                return NotFound();
            }

            var recordVM = _mapper.Map<RecordViewModel>(record);
            return View(recordVM);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var record = await _recordRepo.GetByIdAsync((int)id);
            if (record != null)
            {
                record.Available = false;
                await _recordRepo.UpdateAsync(record);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RecordExists(int id)
        {
            return (_recordRepo.GetTableNoTracking().Any(e => e.Id == id));
        }
    }
}
