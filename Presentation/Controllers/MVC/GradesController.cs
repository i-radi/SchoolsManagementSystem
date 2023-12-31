﻿namespace Presentation.Controllers.MVC
{
    public class GradesController(
        IGradeRepo gradeRepo,
        ISchoolRepo schoolRepo,
        IMapper mapper) : Controller
    {
        private readonly IGradeRepo _gradeRepo = gradeRepo;
        private readonly ISchoolRepo _schoolRepo = schoolRepo;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index(int schoolId)
        {
            var grades = _gradeRepo
                .GetTableNoTracking()
                .Include(g => g.School)
                .OrderByDescending(c => c.Order)
                .ThenBy(c => c.Id)
                .AsQueryable();

            if (schoolId > 0)
            {
                grades = grades.Where(g => g.SchoolId == schoolId);
            }
            var gradesVM = _mapper.Map<List<GradeViewModel>>(await grades.ToListAsync());
            return View(gradesVM);
        }

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

        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

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
