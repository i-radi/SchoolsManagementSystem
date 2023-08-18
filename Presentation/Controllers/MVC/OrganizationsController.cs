namespace Presentation.Controllers.MVC
{
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationRepo _organizationRepo;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrganizationsController(IOrganizationRepo organizationRepo, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _organizationRepo = organizationRepo;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Organizations
        public IActionResult Index(int page = 1, int pageSize = 10, string searchName = "")
        {
            var modelItems = _organizationRepo.GetTableNoTracking();

            if (!string.IsNullOrEmpty(searchName))
            {
                modelItems = modelItems.Where(u => u.Name.Contains(searchName));
            }

            var result = PaginatedList<GetOrganizationDto>.Create(_mapper.Map<List<GetOrganizationDto>>(modelItems), page, pageSize);

            return View(result);
        }

        // GET: Organizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _organizationRepo.GetByIdAsync(id.Value);
            var dto = _mapper.Map<GetOrganizationDto>(modelItem);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // GET: Organizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Organizations/Create
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
                organization.PicturePath = await Picture.Upload(viewmodel.Picture, _webHostEnvironment);
            }
            var model = await _organizationRepo.AddAsync(organization);
            return RedirectToAction(nameof(Index));
        }

        // GET: Organizations/Edit/5
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

        // POST: Organizations/Edit/5
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
                    updatedOrganization.PicturePath = await Picture.Upload(organization.Picture, _webHostEnvironment);
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

        // GET: Organizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _organizationRepo.GetByIdAsync(id.Value);
            var dto = _mapper.Map<GetOrganizationDto>(modelItem);
            if (dto == null)
            {
                return NotFound();
            }

            return View(dto);
        }

        // POST: Organizations/Delete/5
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
