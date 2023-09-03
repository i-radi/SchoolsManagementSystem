using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;

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
        private readonly IUserOrganizationRepo _userOrganizationRepo;
        private readonly IUserRoleRepo _userRoleRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly IActivityRepo _activityRepo;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsersController(
            ILogger<UsersController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOrganizationRepo organizationRepo,
            IUserOrganizationRepo userOrganizationRepo,
            IUserRoleRepo userRoleRepo,
            ISchoolRepo schoolRepo,
            IActivityRepo activityRepo,
            IAuthService authService,
            IMapper mapper,
            ApplicationDBContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organizationRepo = organizationRepo;
            _userOrganizationRepo = userOrganizationRepo;
            _userRoleRepo = userRoleRepo;
            _schoolRepo = schoolRepo;
            _activityRepo = activityRepo;
            _logger = logger;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Users
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchName = "", string searchRole = "", int searchOrg = 0)
        {
            IQueryable<User> usersQuery = _userManager.Users
                .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
                .AsQueryable();

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

            if (searchOrg != 0)
            {
                usersQuery = usersQuery.Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == searchOrg));
            }

            var result = PaginatedList<UserViewModel>.Create(_mapper.Map<List<UserViewModel>>(usersQuery), pageNumber, pageSize);

            var roles = await _roleManager.Roles.ToListAsync();
            ViewBag.RolesList = new SelectList(roles, "Name", "Name");
            ViewBag.OrganizationsList = new SelectList(_organizationRepo.GetTableNoTracking(), "Id", "Name");

            return View(result);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserFormViewModel user)
        {
            string CreatedEmail = (string.IsNullOrEmpty(user.Email)) ? Guid.NewGuid() + "@sms.com" : user.Email;
            var newUser = new User
            {
                Email = CreatedEmail,
                UserName = CreatedEmail.Split('@')[0],
                Name = user.Name,
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
                Address = user.Address,
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                GpsLocation = user.GpsLocation,
                Notes = user.Notes,
                FirstMobile = user.FirstMobile,
                SecondMobile = user.SecondMobile,
                FatherMobile = user.FatherMobile,
                MentorName = user.MentorName,
                MotherMobile = user.MotherMobile,
                SchoolUniversityJob = user.SchoolUniversityJob,
                NationalID = user.NationalID
            };
            if (user.ProfilePicture is not null)
            {
                var fileExtension = Path.GetExtension(Path.GetFileName(user.ProfilePicture.FileName));

                newUser.ProfilePicturePath = await Picture.Upload(
                    user.ProfilePicture,
                    _webHostEnvironment,
                    $"uploads/users/{newUser.UserName}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
            }
            else
            {
                newUser.ProfilePicturePath = "uploads/users/emptyAvatar.png";
            }

            var result = await _userManager.CreateAsync(newUser, newUser.PlainPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            createdUser!.ParticipationNumber = createdUser.Id;
            createdUser!.ParticipationQRCodePath = QR.Generate(createdUser.Id, _webHostEnvironment);
            _context.User.Update(createdUser);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Users");
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.Value);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");

            var viewModel = new UserFormViewModel
            {
                Id = id.Value,
                Name = user.Name,
                Email = user.Email,
                ProfilePicturePath = user.ProfilePicturePath,
                Address = user.Address,
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                GpsLocation = user.GpsLocation,
                Notes = user.Notes,
                FirstMobile = user.FirstMobile,
                SecondMobile = user.SecondMobile,
                FatherMobile = user.FatherMobile,
                MentorName = user.MentorName,
                MotherMobile = user.MotherMobile,
                SchoolUniversityJob = user.SchoolUniversityJob,
                NationalID = user.NationalID,
            };
            return View(viewModel);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserFormViewModel userVM)
        {
            if (id != userVM.Id)
            {
                return NotFound();
            }
            userVM.Email = (string.IsNullOrEmpty(userVM.Email)) ? Guid.NewGuid() + "@sms.com" : userVM.Email;
            string oldEmail = (await _userManager.FindByIdAsync(id.ToString()))!.Email!;

            if (userVM.Email != oldEmail)
            {

                var alreadyexisted = await _userManager.FindByEmailAsync(userVM.Email);
                if (alreadyexisted is not null)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, The user email is exited before.");
                    return View(userVM);
                }
            }

            var updatedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (updatedUser is not null)
            {
                updatedUser.UserName = userVM.Email.Split('@')[0];
                updatedUser.Name = userVM.Name;
                updatedUser.Address = userVM.Address;
                updatedUser.Birthdate = userVM.Birthdate;
                updatedUser.Gender = userVM.Gender;
                updatedUser.GpsLocation = userVM.GpsLocation;
                updatedUser.Notes = userVM.Notes;
                updatedUser.FirstMobile = userVM.FirstMobile;
                updatedUser.SecondMobile = userVM.SecondMobile;
                updatedUser.FatherMobile = userVM.FatherMobile;
                updatedUser.MentorName = userVM.MentorName;
                updatedUser.MotherMobile = userVM.MotherMobile;
                updatedUser.SchoolUniversityJob = userVM.SchoolUniversityJob;
                updatedUser.NationalID = userVM.NationalID;
                if (userVM.ProfilePicture is not null)
                {
                    var fileExtension = Path.GetExtension(Path.GetFileName(userVM.ProfilePicture.FileName));

                    updatedUser.ProfilePicturePath = await Picture.Upload(
                        userVM.ProfilePicture,
                        _webHostEnvironment,
                        $"uploads/users/{updatedUser.UserName}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
                }
                try
                {
                    await _userManager.UpdateAsync(updatedUser);
                    return RedirectToAction(nameof(Index), "Users");
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return View(userVM);

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

            var result = _mapper.Map<UserViewModel>(modelItem);
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

            var result = _mapper.Map<UserViewModel>(modelItem);
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
        public async Task<IActionResult> DeleteRole(int userId, string roleName)
        {

            var userrole = await _userRoleRepo
                .GetTableAsTracking()
                .Include(ur => ur.Role)
                .Where(ur => ur.UserId == userId
                && ur.Role!.Name == roleName)
                .FirstOrDefaultAsync();

            if (userrole == null)
            {
                return NotFound();
            }
            await _userRoleRepo.DeleteAsync(userrole);

            return RedirectToAction("Roles", new { id = userId });
        }
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

        public async Task<IActionResult> UserOrganization(int? id)
        {
            var userOrgs = await _userOrganizationRepo.GetTableNoTracking()
                .Include(uo => uo.Organization)
                .Where(uo => uo.UserId == id).ToListAsync();

            var viewmodels = new List<OrganizationViewModel>();
            foreach (var userOrg in userOrgs)
            {
                viewmodels.Add(new OrganizationViewModel
                {
                    Id = userOrg.Organization?.Id ?? 0,
                    Name = userOrg.Organization?.Name ?? "",
                    PicturePath = userOrg.Organization?.PicturePath ?? ""
                });
            }
            ViewBag.UserId = id;
            return View(viewmodels);
        }
        public async Task<IActionResult> DeleteOrganization(int userId, int orgId)
        {

            var userOrg = await _userOrganizationRepo
                                    .GetTableNoTracking()
                                    .Where(u => u.OrganizationId == orgId && u.UserId == userId)
                                    .FirstOrDefaultAsync();
            if (userOrg == null)
            {
                return NotFound();
            }
            await _userOrganizationRepo.DeleteAsync(userOrg);

            return RedirectToAction("UserOrganization", new { id = userId });

        }
        public async Task<IActionResult> AssignOrganization(int userId)
        {
            var roles = (await _roleManager.Roles.Select(r => r.Name)
                .ToListAsync())
                .AsEnumerable();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userRoles = (await _userManager.GetRolesAsync(user!)).AsEnumerable();

            var userOrgIds = await _userOrganizationRepo
                .GetTableNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => u.OrganizationId)
                .ToListAsync();

            var orgs = await _organizationRepo.GetTableNoTracking()
                .Where(u => !userOrgIds.Contains(u.Id))
                .Select(o => new { OrganizationId = o.Id, o.Name }).ToListAsync();

            var AssignOrgViewModel = new AssignOrganizationViewModel
            {
                UserId = userId,
                UserName = user!.Name,
                OrganizationOptions = new SelectList(orgs, "OrganizationId", "Name")
            };

            return View(AssignOrgViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignOrganization(AssignOrganizationViewModel viewModel)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == viewModel.UserId);
            if (user is not null)
            {
                foreach (var selectedOrgId in viewModel.SelectedOrganizationIds)
                {
                    var userOrg = new UserOrganization
                    {
                        UserId = viewModel.UserId,
                        OrganizationId = selectedOrgId
                    };
                    await _userOrganizationRepo.AddAsync(userOrg);
                }

                return RedirectToAction("UserOrganization", new { id = viewModel.UserId });
            }

            return BadRequest();
        }

    }
}
