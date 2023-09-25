namespace Presentation.Controllers.MVC
{
    public class CoursesController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IOrganizationRepo _organizationRepo;
        private readonly BaseSettings _baseSettings;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICourseDetailsRepo _courseDetailsRepo;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public CoursesController(
            ICourseRepo courseRepo,
            ISchoolRepo schoolRepo,
            IOrganizationRepo organizationRepo,
            BaseSettings baseSettings,
            IWebHostEnvironment webHostEnvironment,
            ICourseDetailsRepo courseDetailsRepo,
            IMapper mapper,
            IAttachmentService attachmentService)
        {
            _courseRepo = courseRepo;
            _schoolRepo = schoolRepo;
            _organizationRepo = organizationRepo;
            _baseSettings = baseSettings;
            _webHostEnvironment = webHostEnvironment;
            _courseDetailsRepo = courseDetailsRepo;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public async Task<IActionResult> Index(int schoolId = 0)
        {
            var courses = _courseRepo
                .GetTableNoTracking()
                .Include(g => g.School)
                .Include(g => g.CourseDetails)
                .AsQueryable();

            if (schoolId > 0)
            {
                courses = courses.Where(g => g.SchoolId == schoolId);
            }
            var coursesVM = _mapper.Map<List<CourseViewModel>>(await courses.ToListAsync());

            ViewBag.SchoolsList = new SelectList(_schoolRepo.GetTableNoTracking(), "Id", "Name");
            return View(coursesVM);
        }

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

        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View(new CourseViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel courseVM)
        {
            if (ModelState.IsValid)
            {
                var course = _mapper.Map<Course>(courseVM);
                course.CourseDetails = new CourseDetails
                {
                    ContentType = courseVM.ContentType,
                    Content = courseVM.Content ?? "",
                };
                if (courseVM.Attachment is not null)
                {
                    var fileExtension = Path.GetExtension(Path.GetFileName(courseVM.Attachment.FileName));

                    course.CourseDetails.Content = await _attachmentService.Upload(courseVM.Attachment, _webHostEnvironment,
                        _baseSettings.attachmentsPath,
                        $"{course.Name}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                }

                await _courseRepo.AddAsync(course);
                return RedirectToAction(nameof(Index));
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name", courseVM.SchoolId);
            return View(courseVM);
        }

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
                    course.CourseDetails = null;
                    if (courseVM.Attachment is not null)
                    {
                        var fileExtension = Path.GetExtension(Path.GetFileName(courseVM.Attachment.FileName));

                        courseVM.Content = await _attachmentService.Upload(courseVM.Attachment, _webHostEnvironment,
                            _baseSettings.attachmentsPath,
                            $"{course.Name}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                    }
                    await _courseRepo.UpdateAsync(course);

                    var courseDetails = await _courseDetailsRepo
                        .GetTableNoTracking()
                        .FirstOrDefaultAsync(cd => cd.CourseId == course.Id);
                    if (courseDetails is null)
                    {
                        return NotFound();
                    }
                    courseDetails.ContentType = courseVM.ContentType;
                    courseDetails.Content = courseVM.Content ?? "";

                    await _courseDetailsRepo.UpdateAsync(courseDetails);
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

        public async Task<IActionResult> DownloadAttachment(int id)
        {
            var course = await _courseRepo.GetByIdAsync(id);

            if (course == null || string.IsNullOrEmpty(course.CourseDetails.Content))
            {
                return NotFound();
            }

            var path = Path.Combine(_webHostEnvironment.WebRootPath, _baseSettings.attachmentsPath, course.CourseDetails.Content);
            string fileName = Path.GetFileName(path);

            string contentType = GetContentType(fileName);

            byte[] fileContents;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, (int)fs.Length);
            }

            return File(fileContents, contentType, fileName);
        }

        private string GetContentType(string fileName)
        {
            string contentType;
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/msword";
                    break;
                case ".ppt":
                case ".pptx":
                    contentType = "application/vnd.ms-powerpoint";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }
            return contentType;
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolsByOrganization(int organizationId)
        {
            List<School> schools = await _schoolRepo
                .GetTableNoTracking()
                .Where(s => s.OrganizationId == organizationId)
                .ToListAsync();

            var schoolList = schools.Select(school => new SelectListItem
            {
                Value = school.Id.ToString(),
                Text = school.Name
            }).ToList();

            return Json(schoolList);
        }
    }
}
