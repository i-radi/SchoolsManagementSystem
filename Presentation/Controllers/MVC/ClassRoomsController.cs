namespace Presentation.Controllers.MVC
{
    public class ClassroomsController : Controller
    {
        private readonly IClassroomRepo _classroomRepo;
        private readonly IGradeRepo _gradeRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BaseSettings _baseSettings;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public ClassroomsController(
            IClassroomRepo classroomRepo,
            IGradeRepo gradeRepo,
            ISchoolRepo schoolRepo,
            IWebHostEnvironment webHostEnvironment,
            BaseSettings baseSettings,
            IMapper mapper,
            IAttachmentService attachmentService)
        {
            _classroomRepo = classroomRepo;
            _gradeRepo = gradeRepo;
            _schoolRepo = schoolRepo;
            _webHostEnvironment = webHostEnvironment;
            _baseSettings = baseSettings;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<IActionResult> Index(int gradeId)
        {
            var classrooms = _classroomRepo
                .GetTableNoTracking()
                .Include(c => c.Grade)
                .OrderByDescending(c => c.Order)
                .ThenBy(c => c.Id)
                .AsQueryable();

            if (gradeId > 0)
            {
                classrooms = classrooms.Where(c => c.GradeId == gradeId);
            }

            var gradesVM = _mapper.Map<List<ClassroomViewModel>>(await classrooms.ToListAsync());
            return View(gradesVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _classroomRepo.GetByIdAsync(id.Value);
            if (classroom == null)
            {
                return NotFound();
            }

            var classroomVM = _mapper.Map<ClassroomViewModel>(classroom);
            return View(classroomVM);
        }

        public IActionResult Create()
        {
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClassroomFormViewModel viewmodel)
        {
            var classroom = new Classroom
            {
                Name = viewmodel.Name,
                GradeId = viewmodel.GradeId
            };

            if (viewmodel.Picture is not null)
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(viewmodel.Picture.FileName));

                var school = _schoolRepo.GetTableNoTracking()
                    .Include(s => s.Organization)
                    .Where(s => s.Grades.Any(g => g.Id == viewmodel.GradeId))
                    .FirstOrDefault();
                var schoolName = school!.Name;
                var orgName = school!.Organization!.Name;
                classroom.PicturePath = await _attachmentService.Upload(viewmodel.Picture, _webHostEnvironment,
                           _baseSettings.classroomsPath,
                    $"{orgName}-{schoolName}-{viewmodel.Name}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
            }

            if (viewmodel.TeacherImage is not null)
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(viewmodel.TeacherImage.FileName));

                var school = _schoolRepo.GetTableNoTracking()
                    .Include(s => s.Organization)
                    .Where(s => s.Grades.Any(g => g.Id == viewmodel.GradeId))
                    .FirstOrDefault();
                var schoolName = school!.Name;
                var orgName = school!.Organization!.Name;

                classroom.TeacherImagePath = await _attachmentService.Upload(viewmodel.TeacherImage, _webHostEnvironment,
                           _baseSettings.classroomsPath,
                    $"{orgName}-{schoolName}-{viewmodel.Name}-Teacher-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
            }

            if (viewmodel.StudentImage is not null)
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(viewmodel.StudentImage.FileName));

                var school = _schoolRepo.GetTableNoTracking()
                    .Include(s => s.Organization)
                    .Where(s => s.Grades.Any(g => g.Id == viewmodel.GradeId))
                    .FirstOrDefault();
                var schoolName = school!.Name;
                var orgName = school!.Organization!.Name;

                classroom.StudentImagePath = await _attachmentService.Upload(viewmodel.StudentImage, _webHostEnvironment,
                        _baseSettings.classroomsPath,
                    $"{orgName}-{schoolName}-{viewmodel.Name}-Student-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
            }

            await _classroomRepo.AddAsync(classroom);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _classroomRepo.GetByIdAsync(id.Value);
            if (classroom == null)
            {
                return NotFound();
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classroom.GradeId);

            var viewModel = new ClassroomFormViewModel
            {
                Id = id.Value,
                Name = classroom.Name,
                GradeId = classroom.GradeId,
                PicturePath = classroom.PicturePath,
                Location = classroom.Location,
                Order = classroom.Order,
                StudentImagePath = classroom.StudentImagePath,
                TeacherImagePath = classroom.TeacherImagePath,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClassroomFormViewModel classroomVM)
        {
            if (id != classroomVM.Id)
            {
                return NotFound();
            }
            var updatedClassroom = await _classroomRepo.GetByIdAsync(id);
            if (updatedClassroom is not null)
            {
                updatedClassroom.Name = classroomVM.Name;
                updatedClassroom.GradeId = classroomVM.GradeId;
                updatedClassroom.Location = classroomVM.Location;
                updatedClassroom.Order = classroomVM.Order;

                if (classroomVM.Picture is not null)
                {
                    var fileExtension = Path.GetExtension(Path.GetFileName(classroomVM.Picture.FileName));

                    var school = _schoolRepo.GetTableNoTracking()
                        .Include(s => s.Organization)
                        .Where(s => s.Grades.Any(g => g.Id == updatedClassroom.GradeId))
                        .FirstOrDefault();
                    var schoolName = school!.Name;
                    var orgName = school!.Organization!.Name;

                    updatedClassroom.PicturePath = await _attachmentService.Upload(classroomVM.Picture, _webHostEnvironment,
                               _baseSettings.classroomsPath,
                    $"{orgName}-{schoolName}-{classroomVM.Name}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                }
                if (classroomVM.TeacherImage is not null)
                {
                    var fileExtension = Path.GetExtension(Path.GetFileName(classroomVM.TeacherImage.FileName));

                    var school = _schoolRepo.GetTableNoTracking()
                        .Include(s => s.Organization)
                        .Where(s => s.Grades.Any(g => g.Id == updatedClassroom.GradeId))
                        .FirstOrDefault();
                    var schoolName = school!.Name;
                    var orgName = school!.Organization!.Name;

                    updatedClassroom.TeacherImagePath = await _attachmentService.Upload(classroomVM.TeacherImage, _webHostEnvironment,
                               _baseSettings.classroomsPath,
                    $"{orgName}-{schoolName}-{classroomVM.Name}-Teacher-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                }
                if (classroomVM.StudentImage is not null)
                {
                    var fileExtension = Path.GetExtension(Path.GetFileName(classroomVM.StudentImage.FileName));

                    var school = _schoolRepo.GetTableNoTracking()
                        .Include(s => s.Organization)
                        .Where(s => s.Grades.Any(g => g.Id == updatedClassroom.GradeId))
                        .FirstOrDefault();
                    var schoolName = school!.Name;
                    var orgName = school!.Organization!.Name;

                    updatedClassroom.StudentImagePath = await _attachmentService.Upload(classroomVM.StudentImage, _webHostEnvironment,
                               _baseSettings.classroomsPath,
                    $"{orgName}-{schoolName}-{classroomVM.Name}-Student-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                }

                try
                {
                    await _classroomRepo.UpdateAsync(updatedClassroom);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableAsTracking().ToList(), "Id", "Name", classroomVM.GradeId);
            return View(classroomVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var classroom = await _classroomRepo.GetByIdAsync(id.Value);
            if (classroom == null)
            {
                return NotFound();
            }
            var classroomVM = _mapper.Map<ClassroomViewModel>(classroom);

            return View(classroomVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var classroom = await _classroomRepo.GetByIdAsync(id);
            if (classroom != null)
            {
                await _classroomRepo.DeleteAsync(classroom);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ClassroomExists(int id)
        {
            return (_classroomRepo.GetTableNoTracking().ToList().Any(e => e.Id == id));
        }
    }
}
