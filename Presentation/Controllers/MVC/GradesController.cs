namespace Presentation.Controllers.MVC
{
    public class GradesController : Controller
    {
        private readonly IGradeRepo _gradeRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IMapper _mapper;

        public GradesController(
            IGradeRepo gradeRepo,
            ISchoolRepo schoolRepo,
            IMapper mapper)
        {
            _gradeRepo = gradeRepo;
            _schoolRepo = schoolRepo;
            _mapper = mapper;
        }

        // GET: Grades
        public async Task<IActionResult> Index(int schoolId)
        {
            var grades = _gradeRepo.GetTableNoTracking().Include(g => g.School).AsQueryable();
            if (schoolId > 0)
            {
                grades = grades.Where(g => g.SchoolId == schoolId);
            }
            var gradesVM = _mapper.Map<List<GradeViewModel>>(await grades.ToListAsync());
            return View(gradesVM);
        }

        // GET: Grades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _gradeRepo.GetByIdAsync(id.Value);
            if (grade == null)
            {
                return NotFound();
            }

            var gradeVM = _mapper.Map<GradeViewModel>(grade);
            return View(gradeVM);
        }

        // GET: Grades/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Grades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeViewModel gradeVM)
        {
            if (ModelState.IsValid)
            {
                var grade = _mapper.Map<Grade>(gradeVM);

                await _gradeRepo.AddAsync(grade);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", gradeVM.SchoolId);
            return View(gradeVM);
        }

        // GET: Grades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _gradeRepo.GetByIdAsync(id.Value);
            if (grade == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", grade.SchoolId);
            var gradeVM = _mapper.Map<GradeViewModel>(grade);
            return View(gradeVM);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradeViewModel gradeVM)
        {
            if (id != gradeVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var grade = _mapper.Map<Grade>(gradeVM);
                    await _gradeRepo.UpdateAsync(grade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(gradeVM.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", gradeVM.SchoolId);
            return View(gradeVM);
        }

        // GET: Grades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _gradeRepo.GetByIdAsync(id.Value);
            if (grade == null)
            {
                return NotFound();
            }

            var gradeVM = _mapper.Map<GradeViewModel>(grade);
            return View(gradeVM);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _gradeRepo.GetByIdAsync((int)id);
            if (grade != null)
            {
                await _gradeRepo.DeleteAsync(grade);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool GradeExists(int id)
        {
            return (_gradeRepo.GetTableNoTracking().Any(e => e.Id == id));
        }
    }
}
