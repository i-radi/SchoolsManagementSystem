namespace Presentation.Controllers.MVC
{
    public class OrganizationsController : Controller
    {
        private readonly IAttachmentService _attachmentService;
        private readonly BaseSettings _baseSettings;
        private readonly IOrganizationRepo _organizationRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrganizationsController(IAttachmentService attachmentService, BaseSettings baseSettings, IOrganizationRepo organizationRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _attachmentService = attachmentService;
            _baseSettings = baseSettings;
            _organizationRepo = organizationRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(int page = 1, int pageSize = 10, string searchName = "")
        {
            var modelItems = _organizationRepo.GetTableNoTracking();

            if (!string.IsNullOrEmpty(searchName))
            {
                modelItems = modelItems.Where(u => u.Name.Contains(searchName));
            }

            var result = PaginatedList<OrganizationViewModel>.Create(_mapper.Map<List<OrganizationViewModel>>(modelItems), page, pageSize);

            return View(result);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _organizationRepo.GetByIdAsync(id.Value);
            var dto = _mapper.Map<OrganizationViewModel>(modelItem);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrganizationFormViewModel viewmodel)
        {
            var organization = new Organization
            {
                Name = viewmodel.Name,
            };
            if (viewmodel.Picture is not null)
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(viewmodel.Picture.FileName));
                organization.PicturePath = await _attachmentService.Upload(viewmodel.Picture, _webHostEnvironment,
                    _baseSettings.organizationsPath,
                    $"{viewmodel.Name}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
            }
            var model = await _organizationRepo.AddAsync(organization);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _organizationRepo.GetByIdAsync(id.Value);
            if (modelItem == null)
            {
                return NotFound();
            }
            var viewModel = new OrganizationFormViewModel
            {
                Id = id.Value,
                Name = modelItem.Name,
                PicturePath = modelItem.PicturePath
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrganizationFormViewModel organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }
            var updatedOrganization = await _organizationRepo.GetByIdAsync(id);
            if (updatedOrganization is not null)
            {
                updatedOrganization.Name = organization.Name;
                if (organization.Picture is not null)
                {
                    var fileExtension = Path.GetExtension(Path.GetFileName(organization.Picture.FileName));
                    updatedOrganization.PicturePath = await _attachmentService.Upload(organization.Picture, _webHostEnvironment,
                        _baseSettings.organizationsPath,
                        $"{organization.Name}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                }
                try
                {
                    await _organizationRepo.UpdateAsync(updatedOrganization);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(organization);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _organizationRepo.GetByIdAsync(id.Value);
            var dto = _mapper.Map<OrganizationViewModel>(modelItem);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organization = await _organizationRepo.GetByIdAsync(id);
            if (organization != null)
            {
                await _organizationRepo.DeleteAsync(organization);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
