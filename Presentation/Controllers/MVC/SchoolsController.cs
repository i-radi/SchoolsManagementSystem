namespace Presentation.Controllers.MVC
{
    public class SchoolsController : Controller
    {
        private readonly ISchoolRepo _schoolsRepo;
        private readonly IOrganizationRepo _organizationRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;


        public SchoolsController(ISchoolRepo schoolsRepo, IOrganizationRepo organizationRepo, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _schoolsRepo = schoolsRepo;
            _organizationRepo = organizationRepo;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        // GET: Schools
        public IActionResult Index(int page = 1, int pageSize = 10, int organizationId = 0)
        {
            var modelItems = _schoolsRepo.GetTableNoTracking()
                .Include(s => s.Organization)
                .AsQueryable();

            if (organizationId > 0)
            {
                modelItems = modelItems.Where(s => s.OrganizationId == organizationId);
            }
            var result = PaginatedList<GetSchoolDto>.Create(_mapper.Map<List<GetSchoolDto>>(modelItems), page, pageSize);

            return View(result);
        }

        // GET: Schools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _schoolsRepo.GetByIdAsync(id.Value);
            var dto = _mapper.Map<GetSchoolDto>(modelItem);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // GET: Schools/Create
        public IActionResult Create()
        {
            var organizations = _organizationRepo.GetTableNoTracking().ToList();
            var viewModel = new CreateSchoolViewModel
            {
                OrganizationOptions = new SelectList(organizations, "Id", "Name")
            };
            return View(viewModel);
        }

        // POST: Schools/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSchoolViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var school = new School
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    OrganizationId = viewModel.OrganizationId
                };
                if (viewModel.Picture is not null)
                {
                    school.PicturePath = await Picture.Upload(viewModel.Picture, _webHostEnvironment);
                }

                await _schoolsRepo.AddAsync(school);
                return RedirectToAction(nameof(Index));
            }

            viewModel.OrganizationOptions = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View(viewModel);
        }

        // GET: Schools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _schoolsRepo.GetByIdAsync(id.Value);
            var viewModel = new UpdateSchoolViewModel
            {
                Id = modelItem.Id,
                Name = modelItem.Name,
                Description = modelItem.Description,
                OrganizationId = modelItem.OrganizationId,
                PicturePath = modelItem.PicturePath,
                OrganizationOptions = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name", modelItem.OrganizationId)
            };

            return View(viewModel);
        }

        // POST: Schools/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateSchoolViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var modelItem = await _schoolsRepo.GetByIdAsync(viewModel.Id);

                if (modelItem is null)
                    return NotFound();

                modelItem.Name = viewModel.Name;
                modelItem.Description = viewModel.Description;
                modelItem.OrganizationId = viewModel.OrganizationId;
                modelItem.PicturePath = viewModel.PicturePath;

                var model = _schoolsRepo.UpdateAsync(modelItem);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Schools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _schoolsRepo.GetByIdAsync(id.Value);
            var dto = _mapper.Map<GetSchoolDto>(modelItem);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organization = await _schoolsRepo.GetByIdAsync(id);
            if (organization != null)
            {
                await _schoolsRepo.DeleteAsync(organization);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
