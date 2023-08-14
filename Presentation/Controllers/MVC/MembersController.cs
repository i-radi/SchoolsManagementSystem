using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;

namespace Presentation.Controllers.MVC
{
    public class MembersController : Controller
    {
        private readonly ILogger<MembersController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IOrganizationRepo _organizationRepo;
        private readonly ISchoolRepo _schoolRepo;
        private readonly ISeasonRepo _seasonRepo;
        private readonly IUserTypeRepo _userTypeRepo;
        private readonly IClassRoomRepo _classRoomRepo;
        private readonly IUserClassRepo _userClassRepo;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public MembersController(
            ILogger<MembersController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOrganizationRepo organizationService,
            ISchoolRepo schoolService,
            ISeasonRepo seasonRepo,
            IUserTypeRepo userTypeRepo,
            IClassRoomRepo classRoomRepo,
            IUserClassRepo userClassRepo,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _organizationRepo = organizationService;
            _schoolRepo = schoolService;
            _seasonRepo = seasonRepo;
            _userTypeRepo = userTypeRepo;
            _classRoomRepo = classRoomRepo;
            _userClassRepo = userClassRepo;
            _logger = logger;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        // GET: Members
        public IActionResult Index(int schoolId, string search = "")
        {
            var users = _userManager.Users
                .Include(u => u.Organization)
                .Include(u => u.UserClasses)
                .ThenInclude(uc => uc.ClassRoom)
                .Include(u => u.UserClasses)
                .ThenInclude(uc => uc.Season)
                .Include(u => u.UserClasses)
                .ThenInclude(uc => uc.UserType)
                .AsQueryable();

            if (schoolId > 0)
            {
                users = users.Where(u => u.SchoolId == schoolId);
            }

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.Name.Contains(search)
                | u.UserClasses.Any(ur => ur.Season.To.ToString().Contains(search))
                | u.UserClasses.Any(ur => ur.ClassRoom.Name.Contains(search))
                | u.UserClasses.Any(ur => ur.UserType.Name.Contains(search)));
            }

            return View(users.ToList());
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Organization).FirstOrDefaultAsync(u => u.Id == id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View();
        }

        // POST: Members/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            var newUser = new User
            {
                UserName = user.Email,
                Email = user.Email,
                Name = user.Name,
                SchoolId = user.SchoolId,
                OrganizationId = user.OrganizationId,
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
            };
            await _userManager.CreateAsync(newUser, newUser.PlainPassword);
            return RedirectToAction(nameof(Index), new { schoolId = newUser.SchoolId });
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Organization).FirstOrDefaultAsync(u => u.Id == id.Value);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name", user.OrganizationId);
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View(user);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            var updatedUser = await _userManager.Users.Include(u => u.Organization).FirstOrDefaultAsync(u => u.Id == id);
            if (updatedUser is not null)
            {
                updatedUser.UserName = user.Email;
                updatedUser.Email = user.Email;
                updatedUser.Name = user.Name;
                updatedUser.SchoolId = user.SchoolId;
                updatedUser.OrganizationId = user.OrganizationId;

                try
                {
                    await _userManager.UpdateAsync(updatedUser);
                    return RedirectToAction(nameof(Index), new { schoolId = updatedUser.SchoolId });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
            return View(user);

        }

        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.Include(u => u.Organization).FirstOrDefaultAsync(u => u.Id == id.Value);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.Users.Include(u => u.Organization).FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index), new { schoolId = user?.SchoolId });
        }

        // GET: Members/Assign
        public async Task<IActionResult> Assign(int id)
        {
            var user = await _userManager.Users.Include(u => u.Organization).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var classrooms = _classRoomRepo.GetTableNoTracking().Include(c => c.Grade).ToList();
            var seasons = _seasonRepo.GetTableNoTracking().ToList();

            var usertypes = _userTypeRepo.GetTableNoTracking().ToList();
            if (user.SchoolId is not null && user.SchoolId > 0)
            {
                classrooms = classrooms.Where(c => c.Grade!.SchoolId == user.SchoolId).ToList();
                seasons = seasons.Where(c => c.SchoolId == user.SchoolId).ToList();
            }
            ViewData["UserId"] = new SelectList(new List<User> { user }, "Id", "Name");
            ViewData["ClassRoomId"] = new SelectList(classrooms, "Id", "Name");
            ViewData["UserTypeId"] = new SelectList(usertypes, "Id", "Name");
            ViewData["SeasonId"] = new SelectList(seasons, "Id", "To");
            return View(new UserClass { UserId = user.Id });
        }

        // POST: Members/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(UserClass addUserClass)
        {
            addUserClass.Id = 0;
            await _userClassRepo.AddAsync(addUserClass);
            var schoolId = (await _userManager.Users.FirstOrDefaultAsync(u => u.Id == addUserClass.UserId))?.SchoolId;
            return RedirectToAction(nameof(Index), new { schoolId = schoolId });
        }

        public IActionResult GetSchools(int organizationId)
        {
            var schools = _schoolRepo.GetTableNoTracking().Where(s => s.OrganizationId == organizationId).ToList();

            var schoolData = schools.Select(s => new
            {
                value = s.Id,
                text = s.Name
            });

            return Json(schoolData);
        }
    }
}
