using AutoMapper;
using Models.Entities;
using Persistance.IRepos;
using Persistance.Repos;

namespace Presentation.MVC_Controllers
{
    public class OrganizationsController : Controller
    {
        private readonly IOrganizationRepo _organizationRepo;
        private readonly IMapper _mapper;

        public OrganizationsController(IOrganizationRepo organizationRepo, IMapper mapper)
        {
            _organizationRepo = organizationRepo;
            _mapper = mapper;
        }

        // GET: Organizations
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var modelItems = _organizationRepo.GetTableNoTracking();
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
        public async Task<IActionResult> Create([Bind("Id,Name")] GetOrganizationDto organization)
        {
            if (ModelState.IsValid)
            {
                var modelItem = _mapper.Map<Organization>(organization);
                var model = await _organizationRepo.AddAsync(modelItem);
                return RedirectToAction(nameof(Index));
            }
            return View(organization);
        }

        // GET: Organizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Organizations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] GetOrganizationDto organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var modelItem = await _organizationRepo.GetByIdAsync(organization.Id);

                if (modelItem is null)
                    return NotFound();

                _mapper.Map(organization, modelItem);

                var model = _organizationRepo.UpdateAsync(modelItem);
                return RedirectToAction(nameof(Index));
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
