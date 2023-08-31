using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static QRCoder.PayloadGenerator;

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
        private readonly IClassroomRepo _classroomRepo;
        private readonly IUserClassRepo _userClassRepo;
        private readonly IAuthService _authService;
        private readonly IExportService<UserViewModel> _exportService;
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
            IClassroomRepo classroomRepo,
            IUserClassRepo userClassRepo,
            IAuthService authService,
            IExportService<UserViewModel> exportService,
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
            _classroomRepo = classroomRepo;
            _userClassRepo = userClassRepo;
            _logger = logger;
            _signInManager = signInManager;
            _authService = authService;
            _exportService = exportService;
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
                .ThenInclude(uc => uc.Classroom)
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
                | u.UserClasses.Any(ur => ur.Classroom!.Name.Contains(search))
                | u.UserClasses.Any(ur => ur.UserType!.Name.Contains(search)));
            }
            var usersVM = _mapper.Map<List<UserViewModel>>(users.ToList());
            return View(usersVM);
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

            var userVM = _mapper.Map<UserViewModel>(user);
            return View(userVM);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
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
                Email = user.Email,
                UserName = user.Email.Split('@')[0],
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
                newUser.ProfilePicturePath = await Picture.Upload(user.ProfilePicture, _webHostEnvironment);
            }
            else
            {
                newUser.ProfilePicturePath = "emptyAvatar.png";
            }

            await _userManager.CreateAsync(newUser, newUser.PlainPassword);

            var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            createdUser!.ParticipationNumber = createdUser.Id;
            createdUser!.ParticipationQRCodePath = QR.Generate(createdUser.Id, _webHostEnvironment);
            _context.User.Update(createdUser);
            await _context.SaveChangesAsync();

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
            var updatedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (updatedUser is not null)
            {
                updatedUser.UserName = userVM.Email;
                updatedUser.Email = userVM.Email;
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
                    updatedUser.ProfilePicturePath = await Picture.Upload(userVM.ProfilePicture, _webHostEnvironment);
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
            return View(userVM);

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
            var userVM = _mapper.Map<UserViewModel>(user);

            return View(userVM);
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
            var classrooms = _classroomRepo.GetTableNoTracking().Include(c => c.Grade).ToList();
            var seasons = _seasonRepo.GetTableNoTracking().ToList();

            var usertypes = _userTypeRepo.GetTableNoTracking().ToList();

            ViewData["UserId"] = new SelectList(new List<User> { user }, "Id", "Name");
            ViewData["ClassroomId"] = new SelectList(classrooms, "Id", "Name");
            ViewData["UserTypeId"] = new SelectList(usertypes, "Id", "Name");
            ViewData["SeasonId"] = new SelectList(seasons, "Id", "Name");
            return View(new UserClassViewModel { UserId = user.Id });
        }

        // POST: Members/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(UserClassViewModel userClassVM)
        {
            userClassVM.Id = 0;
            var userClass = _mapper.Map<UserClass>(userClassVM);
            await _userClassRepo.AddAsync(userClass);
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

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var result = _context.Users.AsQueryable();

            var searchBy = dtParameters.Search?.Value;
            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Email != null && r.
                                           Email.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.PhoneNumber != null && r.PhoneNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Notes != null && r.Notes.ToUpper().Contains(searchBy.ToUpper()));
            }

            if (!string.IsNullOrEmpty(dtParameters.SortOrder))
            {
                result = result.OrderBy(dtParameters.SortOrder);
            }

            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _context.Users.CountAsync();

            var records = result.Skip(dtParameters.Start).Take(dtParameters.Length);

            var usersVM = _mapper.Map<List<UserViewModel>>(records.ToList());

            return Json(new DtResult<UserViewModel>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = usersVM
            });
        }

        [HttpPost]
        public async Task<IActionResult> ExportTable([FromQuery] string format, [FromForm] string dtParametersJson)
        {
            var dtParameters = new DtParameters();
            if (!string.IsNullOrEmpty(dtParametersJson))
            {
                dtParameters = JsonConvert.DeserializeObject<DtParameters>(dtParametersJson);
            }

            if (dtParameters != default)
            {
                var searchBy = dtParameters.Search?.Value;

                var orderCriteria = "Id";
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }

                var result = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(searchBy))
                {
                    result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.Email != null && r.
                                               Email.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.PhoneNumber != null && r.PhoneNumber.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.Notes != null && r.Notes.ToUpper().Contains(searchBy.ToUpper()));
                }

                result = orderAscendingDirection ? result.OrderBy(orderCriteria, DtOrderDir.Asc) : result.OrderBy(orderCriteria, DtOrderDir.Desc);

                var resultUserList = await result.ToListAsync();
                var resultList = _mapper.Map<List<UserViewModel>>(resultUserList.ToList());

                switch (format)
                {
                    case ExportFormat.Excel:
                        return File(
                            await _exportService.ExportToExcel(resultList),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "data.xlsx");

                    case ExportFormat.Csv:
                        return File(_exportService.ExportToCsv(resultList),
                            "application/csv",
                            "data.csv");

                    case ExportFormat.Html:
                        return File(_exportService.ExportToHtml(resultList),
                            "application/csv",
                            "data.html");

                    case ExportFormat.Json:
                        return File(_exportService.ExportToJson(resultList),
                            "application/json",
                            "data.json");

                    case ExportFormat.Xml:
                        return File(_exportService.ExportToXml(resultList),
                            "application/xml",
                            "data.xml");

                    case ExportFormat.Yaml:
                        return File(_exportService.ExportToYaml(resultList),
                            "application/yaml",
                            "data.yaml");
                }
            }

            return null;
        }

    }
}
