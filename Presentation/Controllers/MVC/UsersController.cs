namespace Presentation.Controllers.MVC
{
    [Authorize(Policy = "SuperAdmin")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IOrganizationRepo _organizationRepo;
        private readonly IUserRoleRepo _userRoleRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IActivityRepo _activityRepo;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public UsersController(
            ILogger<UsersController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOrganizationRepo organizationRepo,
            IUserRoleRepo userRoleRepo,
            ISchoolRepo schoolRepo,
            IActivityRepo activityRepo,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organizationRepo = organizationRepo;
            _userRoleRepo = userRoleRepo;
            _schoolRepo = schoolRepo;
            _activityRepo = activityRepo;
            _logger = logger;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }


        // GET: Users
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string searchName = "", string searchRole = "")
        {
            IQueryable<User> usersQuery = _userManager.Users;

            if (!string.IsNullOrEmpty(searchName))
            {
                usersQuery = usersQuery.Where(u => u.Name.Contains(searchName));
            }

            if (!string.IsNullOrEmpty(searchRole))
            {
                var usersWithRole = await _userManager.GetUsersInRoleAsync(searchRole);
                var userIds = usersWithRole.Select(u => u.Id);
                usersQuery = usersQuery.Where(u => userIds.Contains(u.Id));
            }

            var modelItems = PaginatedList<User>.Create(usersQuery, page, pageSize);

            var result = PaginatedList<GetUserDto>.Create(_mapper.Map<List<GetUserDto>>(modelItems), page, pageSize);

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.RolesList = new SelectList(roles, "Name", "Name");

            return View(result);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);

            var result = _mapper.Map<GetUserDto>(modelItem);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelItem = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == id);

            var result = _mapper.Map<GetUserDto>(modelItem);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modelItem = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (modelItem == null)
            {
                return Problem("Entity set 'ApplicationDBContext.User'  is null.");
            }
            await _userManager.DeleteAsync(modelItem);

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Roles/5
        public async Task<IActionResult> Roles(int? id)
        {
            var userRoles = await _userRoleRepo.GetTableNoTracking()
                .Include(ur => ur.User)
                .Include(ur => ur.Role)
                .Include(ur => ur.Activity)
                .Where(ur => ur.UserId == id).ToListAsync();

            var viewmodels = new List<GetRoleViewModel>();
            foreach (var userRole in userRoles)
            {
                viewmodels.Add(new GetRoleViewModel
                {
                    Name = userRole.Role?.Name ?? "",
                    Activity = userRole.Activity?.Name ?? "",
                    Organization = userRole.OrganizationId is not null ?
                    (await _organizationRepo.GetByIdAsync(userRole.OrganizationId.Value)).Name : "",
                    School = userRole.SchoolId is not null ?
                    (await _schoolRepo.GetByIdAsync(userRole.SchoolId.Value)).Name : ""
                });
            }
            ViewBag.UserId = id;
            return View(viewmodels);
        }

        // GET: Users/DeleteRole
        public async Task<IActionResult> DeleteRole(string userId, string roleName)
        {

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                string userIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()!;
                _logger.LogInformation("User IP Address: {UserIpAddress}", userIpAddress);
                return RedirectToAction("Roles", new { id = userId });
            }
            return NotFound();
        }

        // GET: Users/CreateRole/
        public async Task<IActionResult> CreateRole(int userId)
        {
            var roles = (await _roleManager.Roles.Select(r => r.Name)
                .ToListAsync())
                .AsEnumerable();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userRoles = (await _userManager.GetRolesAsync(user!)).AsEnumerable();
            var isSuperAdmin = userRoles.Any(u => u == "SuperAdmin");
            if (isSuperAdmin)
            {
                roles = roles.Except(new List<string> { "SuperAdmin" }.AsEnumerable());
            }

            var orgs = await _organizationRepo.GetTableNoTracking()
                .Select(o => new { OrganizationId = o.Id, o.Name }).ToListAsync();

            var schools = await _schoolRepo.GetTableNoTracking()
                .Select(o => new { SchoolId = o.Id, o.Name }).ToListAsync();

            var activities = await _activityRepo.GetTableNoTracking()
                .Select(o => new { ActivityId = o.Id, Name = o.Name }).ToListAsync();

            var createRoleViewModel = new CreateRoleViewModel
            {
                Id = userId,
                UserName = user!.Email,
                RoleOptions = new SelectList(roles),
                OrganizationOptions = new SelectList(orgs, "OrganizationId", "Name"),
                SchoolOptions = new SelectList(schools, "SchoolId", "Name"),
                ActivityOptions = new SelectList(activities, "ActivityId", "Name")
            };

            return View(createRoleViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel viewModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == viewModel.Id);
            if (user is not null)
            {
                if (viewModel.SchoolId != null)
                {
                    viewModel.OrganizationId = (await _schoolRepo.GetByIdAsync(viewModel.SchoolId.Value)).OrganizationId;
                }
                if (viewModel.ActivityId != null)
                {
                    viewModel.SchoolId = (await _activityRepo.GetByIdAsync(viewModel.ActivityId.Value)).SchoolId;
                    viewModel.OrganizationId = (await _schoolRepo.GetByIdAsync(viewModel.SchoolId.Value)).OrganizationId;
                }
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = (await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == viewModel.RoleName))!.Id,
                    OrganizationId = viewModel.OrganizationId,
                    SchoolId = viewModel.SchoolId,
                    ActivityId = viewModel.ActivityId,
                };
                await _userRoleRepo.UpdateAsync(userRole);

                string userIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString()!;
                _logger.LogInformation("User IP Address: {UserIpAddress}", userIpAddress);
                return RedirectToAction("Roles", new { id = viewModel.Id });

            }
            return BadRequest();
        }
    }
}
