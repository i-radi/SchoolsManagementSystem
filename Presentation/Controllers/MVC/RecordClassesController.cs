using System.Linq.Dynamic.Core;

namespace Presentation.Controllers.MVC
{
    public class RecordClassesController(
        IMapper mapper,
        IRecordRepo recordRepo,
        IClassroomRepo classroomRepo,
        IRecordClassRepo recordClassRepo) : Controller
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRecordRepo _recordRepo = recordRepo;
        private readonly IClassroomRepo _classroomRepo = classroomRepo;
        private readonly IRecordClassRepo _recordClassRepo = recordClassRepo;

        public IActionResult Index(int? recordId)
        {
            var recordClass = _recordClassRepo.GetTableNoTracking()
                .Include(u => u.Classroom)
                .Include(u => u.Record)
                .Where(r => r.Record.Available)
                .AsQueryable();

            if (recordId is not null)
            {
                recordClass = recordClass.Where(u => u.RecordId == recordId);
            }

            ViewBag.ClassroomId = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.RecordId = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available), "Id", "Name");

            var result = _mapper.Map<List<RecordClassViewModel>>(recordClass);
            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordClass = await _recordClassRepo
                .GetTableAsTracking()
                .Include(u => u.Classroom)
                .Include(u => u.Record)
                .Where(r => r.Record.Available)
                .FirstOrDefaultAsync(uc => uc.Id == id);
            if (recordClass == null)
            {
                return NotFound();
            }

            var recordClassVM = _mapper.Map<RecordClassViewModel>(recordClass);
            ViewBag.ClassroomId = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.RecordId = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available), "Id", "Name");

            return View(recordClassVM);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _recordClassRepo
                .GetTableAsTracking()
                .Include(u => u.Classroom)
                .Include(u => u.Record)
                .Where(r => r.Record.Available)
                .FirstOrDefaultAsync(uc => uc.Id == id);

            if (modelItem == null)
            {
                return NotFound();
            }
            var viewModel = new RecordClassViewModel
            {
                Id = id.Value,
                RecordId = modelItem.RecordId,
                ClassroomId = modelItem.ClassroomId,
            };

            ViewBag.ClassroomId = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.RecordId = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available), "Id", "Name");
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecordClassViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }
            var updatedRecordClass = await _recordClassRepo.GetByIdAsync(id);
            if (updatedRecordClass is not null)
            {
                updatedRecordClass.RecordId = viewModel.RecordId;
                updatedRecordClass.ClassroomId = viewModel.ClassroomId;

                try
                {
                    await _recordClassRepo.UpdateAsync(updatedRecordClass);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recordClass = await _recordClassRepo
                .GetTableAsTracking()
                .Include(u => u.Classroom)
                .Include(u => u.Record)
                .FirstOrDefaultAsync(uc => uc.Id == id);
            if (recordClass == null)
            {
                return NotFound();
            }

            var recordClassVM = _mapper.Map<RecordClassViewModel>(recordClass);
            ViewBag.ClassroomId = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.RecordId = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available), "Id", "Name");

            return View(recordClassVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recordClass = await _recordClassRepo
                .GetTableAsTracking()
                .FirstOrDefaultAsync(uc => uc.Id == id);

            if (recordClass == null)
            {
                return NotFound();
            }

            await _recordClassRepo.DeleteAsync(recordClass);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Assign(int? recordId)
        {
            ViewBag.ClassroomId = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.RecordId = new SelectList(_recordRepo.GetTableNoTracking().Where(r => r.Available), "Id", "Name");

            var recordClass = new RecordClassViewModel();
            if (recordId is not null)
            {
                recordClass.RecordId = recordId.Value;
            }

            return View(recordClass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(RecordClassViewModel userClassVM)
        {
            var userClass = _mapper.Map<RecordClass>(userClassVM);
            await _recordClassRepo.AddAsync(userClass);
            return RedirectToAction(nameof(Index));
        }

    }
}
