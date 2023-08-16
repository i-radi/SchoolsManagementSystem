using AutoMapper;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Persistance.IRepos;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using VModels.ViewModels;

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
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

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
            IMapper mapper,
            ApplicationDBContext context,
            IWebHostEnvironment webHostEnvironment)
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
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Members
        public IActionResult Index(int schoolId, string search = "")
        {
            var users = _userManager.Users
                .Include(u => u.UserRoles)
                .Include(u => u.UserClasses)
                .ThenInclude(uc => uc.ClassRoom)
                .Include(u => u.UserClasses)
                .ThenInclude(uc => uc.Season)
                .Include(u => u.UserClasses)
                .ThenInclude(uc => uc.UserType)
                .AsQueryable();

            if (schoolId > 0)
            {
                users = users.Where(u => u.UserRoles.Any(u => u.SchoolId == schoolId));
            }

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.Name.Contains(search)
                | u.UserClasses.Any(ur => ur.Season!.To.ToString().Contains(search))
                | u.UserClasses.Any(ur => ur.ClassRoom!.Name.Contains(search))
                | u.UserClasses.Any(ur => ur.UserType!.Name.Contains(search)));
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

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.Value);
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
        public async Task<IActionResult> Create(UserFormViewModel user)
        {
            var newUser = new User
            {
                UserName = user.Email,
                Email = user.Email,
                Name = user.Name,
                PlainPassword = "123456",
                RefreshToken = Guid.NewGuid(),
                RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(20),
            };
            if (user.ProfilePicture is not null)
            {
                newUser.ProfilePicturePath = await Picture.Upload(user.ProfilePicture, _webHostEnvironment);
            }

            await _userManager.CreateAsync(newUser, newUser.PlainPassword);

            var createdUser = _userManager.FindByEmailAsync(newUser.Email);
            QR.Generate(createdUser.Result!.Id, _webHostEnvironment);

            return RedirectToAction(nameof(Index));
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
            ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");

            var viewModel = new UserFormViewModel
            {
                Id = id.Value,
                Name = user.Name,
                Email = user.Email,
                ProfilePicturePath = user.ProfilePicturePath
            };
            return View(viewModel);
        }

        // POST: Members/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserFormViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            var updatedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (updatedUser is not null)
            {
                updatedUser.UserName = user.Email;
                updatedUser.Email = user.Email;
                updatedUser.Name = user.Name;
                if (user.ProfilePicture is not null)
                {
                    updatedUser.ProfilePicturePath = await Picture.Upload(user.ProfilePicture, _webHostEnvironment);
                }
                try
                {
                    await _userManager.UpdateAsync(updatedUser);
                    return RedirectToAction(nameof(Index));
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

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.Value);
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
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Members/Assign
        public async Task<IActionResult> Assign(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var classrooms = _classRoomRepo.GetTableNoTracking().Include(c => c.Grade).ToList();
            var seasons = _seasonRepo.GetTableNoTracking().ToList();

            var usertypes = _userTypeRepo.GetTableNoTracking().ToList();

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
            return RedirectToAction(nameof(Index));
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

        public IActionResult GenerateQRCode(int userId)
        {
            string userData = userId.ToString();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(userData, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);

            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            MemoryStream memoryStream = new MemoryStream();
            qrCodeImage.Save(memoryStream, ImageFormat.Png);

            return File(memoryStream.ToArray(), "image/png");
        }
    }
}
