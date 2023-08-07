using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;

namespace Presentation.Controllers.MVC
{
    public class GradesController : Controller
    {
        private readonly IGradeRepo _gradeRepo;
        private readonly ISchoolRepo _schoolRepo;

        public GradesController(IGradeRepo gradeRepo, ISchoolRepo schoolRepo)
        {
            _gradeRepo = gradeRepo;
            _schoolRepo = schoolRepo;
        }

        // GET: Grades
        public async Task<IActionResult> Index(int schoolId)
        {
            var grades = _gradeRepo.GetTableNoTracking().Include(g => g.School).AsQueryable();
            if (schoolId > 0 )
            {
                grades = grades.Where(g => g.SchoolId == schoolId);
            }
            return View(await grades.ToListAsync());
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

            return View(grade);
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
        public async Task<IActionResult> Create([Bind("Id,Name,SchoolId")] Grade grade)
        {
            if (ModelState.IsValid)
            {
                await _gradeRepo.AddAsync(grade);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", grade.SchoolId);
            return View(grade);
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
            return View(grade);
        }

        // POST: Grades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SchoolId")] Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _gradeRepo.UpdateAsync(grade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", grade.SchoolId);
            return View(grade);
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

            return View(grade);
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
