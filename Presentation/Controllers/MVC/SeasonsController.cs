﻿namespace Presentation.Controllers.MVC
{
    public class SeasonsController(
        ISeasonRepo seasonRepo,
        ISchoolRepo schoolRepo,
        IMapper mapper) : Controller
    {
        private readonly ISeasonRepo _seasonRepo = seasonRepo;
        private readonly ISchoolRepo _schoolRepo = schoolRepo;
        private readonly IMapper _mapper = mapper;

        public async Task<IActionResult> Index(int schoolId)
        {
            var seasons = _seasonRepo.GetTableNoTracking().Include(s => s.School).AsQueryable();
            if (schoolId > 0)
            {
                seasons = seasons.Where(s => s.SchoolId == schoolId);
            }
            var seasonsVM = _mapper.Map<List<SeasonViewModel>>(await seasons.ToListAsync());
            return View(seasonsVM);
        }

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

            var seasonVM = _mapper.Map<SeasonViewModel>(season);
            return View(seasonVM);
        }

        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeasonViewModel seasonVM)
        {
            if (ModelState.IsValid)
            {
                var season = _mapper.Map<Season>(seasonVM);
                await _seasonRepo.AddAsync(season);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", seasonVM.SchoolId);
            return View(seasonVM);
        }

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
            var seasonVM = _mapper.Map<SeasonViewModel>(season);
            return View(seasonVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SeasonViewModel seasonVM)
        {
            if (id != seasonVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var season = _mapper.Map<Season>(seasonVM);
                    await _seasonRepo.UpdateAsync(season);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonExists(seasonVM.Id))
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
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", seasonVM.SchoolId);
            return View(seasonVM);
        }

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
            var seasonVM = _mapper.Map<SeasonViewModel>(season);

            return View(seasonVM);
        }

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
