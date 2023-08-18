namespace Presentation.Controllers.MVC
{
    public class ClassRoomsController : Controller
    {
        private readonly IClassRoomRepo _classRoomRepo;
        private readonly IGradeRepo _gradeRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClassRoomsController(IClassRoomRepo classRoomRepo, IGradeRepo gradeRepo, IWebHostEnvironment webHostEnvironment)
        {
            _classRoomRepo = classRoomRepo;
            _gradeRepo = gradeRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ClassRooms
        public async Task<IActionResult> Index(int gradeId)
        {
            var classrooms = _classRoomRepo.GetTableNoTracking().Include(c => c.Grade).AsQueryable();
            if (gradeId > 0)
            {
                classrooms = classrooms.Where(c => c.GradeId == gradeId);
            }

            return View(await classrooms.ToListAsync());
        }

        // GET: ClassRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepo.GetByIdAsync(id.Value);
            if (classRoom == null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        // GET: ClassRooms/Create
        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: ClassRooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomFormViewModel viewmodel)
        {
            var classroom = new ClassRoom
            {
                Name = viewmodel.Name,
                GradeId = viewmodel.GradeId
            };

            if (viewmodel.Picture is not null)
            {
                classroom.PicturePath = await Picture.Upload(viewmodel.Picture, _webHostEnvironment);
            }

            await _classRoomRepo.AddAsync(classroom);
            return RedirectToAction(nameof(Index));
        }

        // GET: ClassRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepo.GetByIdAsync(id.Value);
            if (classRoom == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classRoom.GradeId);

            var viewModel = new ClassroomFormViewModel
            {
                Id = id.Value,
                Name = classRoom.Name,
                GradeId = classRoom.GradeId,
                PicturePath = classRoom.PicturePath
            };
            return View(viewModel);
        }

        // POST: ClassRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassroomFormViewModel classRoom)
        {
            if (id != classRoom.Id)
            {
                return NotFound();
            }
            var updatedClassroom = await _classRoomRepo.GetByIdAsync(id);
            if (updatedClassroom is not null)
            {
                updatedClassroom.Name = classRoom.Name;
                updatedClassroom.GradeId = classRoom.GradeId;
                if (classRoom.Picture is not null)
                {
                    updatedClassroom.PicturePath = await Picture.Upload(classRoom.Picture, _webHostEnvironment);
                }
                try
                {
                    await _classRoomRepo.UpdateAsync(updatedClassroom);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classRoom.GradeId);
            return View(classRoom);
        }

        // GET: ClassRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classRoom = await _classRoomRepo.GetByIdAsync(id.Value);
            if (classRoom == null)
            {
                return NotFound();
            }

            return View(classRoom);
        }

        // POST: ClassRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classRoom = await _classRoomRepo.GetByIdAsync(id);
            if (classRoom != null)
            {
                await _classRoomRepo.DeleteAsync(classRoom);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClassRoomExists(int id)
        {
            return (_classRoomRepo.GetTableNoTracking().ToList().Any(e => e.Id == id));
        }
    }
}
