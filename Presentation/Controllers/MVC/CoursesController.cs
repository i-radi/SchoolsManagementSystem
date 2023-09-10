namespace Presentation.Controllers.MVC
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IMapper _mapper;

        public CoursesController(
            ICourseRepo courseRepo,
            ISchoolRepo schoolRepo,
            IMapper mapper)
        {
            _courseRepo = courseRepo;
            _schoolRepo = schoolRepo;
            _mapper = mapper;
        }

        // GET: Courses
        public async Task<IActionResult> Index(int schoolId)
        {
            var courses = _courseRepo
                .GetTableNoTracking()
                .Include(g => g.School)
                .AsQueryable();

            if (schoolId > 0)
            {
                courses = courses.Where(g => g.SchoolId == schoolId);
            }
            var coursesVM = _mapper.Map<List<CourseViewModel>>(await courses.ToListAsync());
            return View(coursesVM);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            var courseVM = _mapper.Map<CourseViewModel>(course);
            return View(courseVM);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Courses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel courseVM)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(courseVM);

                await _courseRepo.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", courseVM.SchoolId);
            return View(courseVM);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", course.SchoolId);
            var courseVM = _mapper.Map<CourseViewModel>(course);
            return View(courseVM);
        }

        // POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CourseViewModel courseVM)
        {
            if (id != courseVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var course = _mapper.Map<Course>(courseVM);
                    await _courseRepo.UpdateAsync(course);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(courseVM.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", courseVM.SchoolId);
            return View(courseVM);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _courseRepo.GetByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }

            var courseVM = _mapper.Map<CourseViewModel>(course);
            return View(courseVM);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _courseRepo.GetByIdAsync((int)id);
            if (course != null)
            {
                await _courseRepo.DeleteAsync(course);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_courseRepo.GetTableNoTracking().Any(e => e.Id == id));
        }
    }
}
