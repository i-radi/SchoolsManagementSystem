using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;

namespace Presentation.Controllers.MVC
{
    public class SeasonsController : Controller
    {
        private readonly ISeasonRepo _seasonRepo;
        private readonly ISchoolRepo _schoolRepo;

        public SeasonsController(ISeasonRepo seasonRepo, ISchoolRepo schoolRepo)
        {
            _seasonRepo = seasonRepo;
            _schoolRepo = schoolRepo;
        }

        // GET: Seasons
        public async Task<IActionResult> Index(int schoolId)
        {
            var seasons = _seasonRepo.GetTableNoTracking().Include(s => s.School).AsQueryable();
            if (schoolId > 0)
            {
                seasons = seasons.Where(s => s.SchoolId == schoolId);
            }
            return View(await seasons.ToListAsync());
        }

        // GET: Seasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _seasonRepo.GetByIdAsync(id.Value);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Seasons/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Seasons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,From,To,IsCurrent,SchoolId")] Season season)
        {
            if (ModelState.IsValid)
            {
                await _seasonRepo.AddAsync(season);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", season.SchoolId);
            return View(season);
        }

        // GET: Seasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _seasonRepo.GetByIdAsync(id.Value);
            if (season == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", season.SchoolId);
            return View(season);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,From,To,IsCurrent,SchoolId")] Season season)
        {
            if (id != season.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _seasonRepo.UpdateAsync(season);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonExists(season.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", season.SchoolId);
            return View(season);
        }

        // GET: Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _seasonRepo.GetByIdAsync(id.Value);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var season = await _seasonRepo.GetByIdAsync(id);
            if (season != null)
            {
                await _seasonRepo.DeleteAsync(season);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SeasonExists(int id)
        {
            return (_seasonRepo.GetTableNoTracking().Any(e => e.Id == id));
        }
    }
}
