using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Entities.Identity;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using System.Drawing.Printing;
using System.Linq;
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
        private readonly IGradeRepo _gradeRepo;
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
            IGradeRepo gradeRepo,
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
            _gradeRepo = gradeRepo;
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
        public IActionResult Index(
            int pageNumber = 1,
            int pageSize = 10,
            int orgId = 0,
            int schoolId = 0,
            int seasonId = 0,
            int gradeId = 0,
            int classroomId = 0,
            int usertypeId = 0,
            string searchUserName = "")
        {
            var userclass = _userClassRepo.GetTableNoTracking()
                .Include(u => u.User)
                .Include(u => u.Classroom)
                .Include(u => u.Season)
                .ThenInclude(s => s.School)
                .Include(u => u.UserType)
                .AsQueryable();

            if (orgId != 0)
            {
                userclass = userclass.Where(uc => uc.Season!.School!.OrganizationId == orgId);
            }
            if (schoolId != 0)
            {
                userclass = userclass.Where(uc => uc.Season!.SchoolId == schoolId);
            }
            if (seasonId != 0)
            {
                userclass = userclass.Where(uc => uc.SeasonId == seasonId);
            }
            if (gradeId != 0)
            {
                userclass = userclass.Where(uc => uc.Classroom!.GradeId == gradeId);
            }
            if (classroomId != 0)
            {
                userclass = userclass.Where(uc => uc.ClassroomId == classroomId);
            }
            if (usertypeId != 0)
            {
                userclass = userclass.Where(uc => uc.UserTypeId == usertypeId);
            }
            if (!string.IsNullOrWhiteSpace(searchUserName))
            {
                userclass = userclass.Where(uc => uc.User!.Email!.ToLower().Contains(searchUserName.ToLower())
                || uc.User!.Name!.ToLower().Contains(searchUserName.ToLower()));
            }

            ViewBag.OrganizationsList = new SelectList(_organizationRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.SchoolsList = new SelectList(_schoolRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.SeasonsList = new SelectList(_seasonRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.GradesList = new SelectList(_gradeRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.ClassroomsList = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
            ViewBag.UserTypesList = new SelectList(_userTypeRepo.GetTableNoTracking(), "Id", "Name");

            var result = PaginatedList<UserClassViewModel>.Create(_mapper.Map<List<UserClassViewModel>>(userclass), pageNumber, pageSize);
            return View(result);
        }

        // GET: Members/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userclass = await _userClassRepo
                .GetTableAsTracking()
                .Include(uc => uc.User)
                .Include(uc => uc.Season)
                .ThenInclude(s => s!.School)
                .ThenInclude(sc => sc!.Organization)
                .Include(uc => uc.Classroom)
                .ThenInclude(c => c.Grade)
                .Include(uc => uc.UserType)
                .FirstOrDefaultAsync(uc => uc.Id == id);
            if (userclass == null)
            {
                return NotFound();
            }

            var userclassVM = _mapper.Map<UserClassViewModel>(userclass);
            userclassVM.OrganizationId = userclassVM.Season!.School!.Organization!.Id;
            userclassVM.Organization = userclassVM.Season.School.Organization;
            userclassVM.GradeId = userclassVM.Classroom!.GradeId;
            userclassVM.Grade = userclassVM.Classroom.Grade;

            return View(userclassVM);
        }
        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userclass = await _userClassRepo
                .GetTableAsTracking()
                .Include(uc => uc.User)
                .Include(uc => uc.Season)
                .ThenInclude(s => s!.School)
                .ThenInclude(sc => sc!.Organization)
                .Include(uc => uc.Classroom)
                .ThenInclude(c => c.Grade)
                .Include(uc => uc.UserType)
                .FirstOrDefaultAsync(uc => uc.Id == id);
            if (userclass == null)
            {
                return NotFound();
            }

            var userclassVM = _mapper.Map<UserClassViewModel>(userclass);
            userclassVM.OrganizationId = userclassVM.Season!.School!.Organization!.Id;
            userclassVM.Organization = userclassVM.Season.School.Organization;
            userclassVM.GradeId = userclassVM.Classroom!.GradeId;
            userclassVM.Grade = userclassVM.Classroom.Grade;

            return View(userclassVM);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userclass = await _userClassRepo
                .GetTableAsTracking()
                .FirstOrDefaultAsync(uc => uc.Id == id);

            if (userclass == null)
            {
                return NotFound();
            }

            await _userClassRepo.DeleteAsync(userclass);

            return RedirectToAction(nameof(Index));
        }

        // GET: Members/Assign
        public async Task<IActionResult> Assign(int? orgid, int? schoolid, int? userId, int? gradeid)
        {
            var organizations = await _organizationRepo.GetTableNoTracking().ToListAsync();
            var schools = new List<School>();
            var users = new List<User>();
            var seasons = new List<Season>();
            var grades = new List<Grade>();
            var classrooms = new List<Classroom>();
            var usertypes = new List<UserType>();
            var userclass = new UserClassViewModel();

            if (orgid is not null)
            {
                userclass.OrganizationId = (int)orgid;
                schools = _schoolRepo.GetTableNoTracking().Where(s => s.OrganizationId == orgid).ToList();
            }

            if (userId is not null)
            {
                userclass.UserId = (int)userId;
            }

            if (schoolid is not null)
            {
                userclass.SchoolId = (int)schoolid;
                userclass.OrganizationId = (int)orgid!;
                grades = _gradeRepo.GetTableNoTracking().Where(g => g.SchoolId == schoolid).ToList();
                users = await _userManager.Users
                    .Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == orgid))
                    .ToListAsync();
            }

            if (gradeid is not null)
            {
                userclass.GradeId = (int)gradeid;
                userclass.SchoolId = (int)schoolid!;
                userclass.OrganizationId = (int)orgid!;
                userclass.UserId = userId ?? 0;
                seasons = _seasonRepo.GetTableNoTracking().Where(s => s.SchoolId == schoolid).ToList();
                classrooms = _classroomRepo.GetTableNoTracking().Where(s => s.GradeId == gradeid).ToList();
                usertypes = _userTypeRepo.GetTableNoTracking().ToList();
            }

            ViewData["OrgId"] = new SelectList(organizations, "Id", "Name");
            ViewData["SchoolId"] = new SelectList(schools, "Id", "Name");
            ViewData["UserId"] = new SelectList(users, "Id", "Name");
            ViewData["GradeId"] = new SelectList(grades, "Id", "Name");
            ViewData["SeasonId"] = new SelectList(seasons, "Id", "Name");
            ViewData["ClassroomId"] = new SelectList(classrooms, "Id", "Name");
            ViewData["UserTypeId"] = new SelectList(usertypes, "Id", "Name");

            return View(userclass);
        }

        // POST: Members/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(UserClassViewModel userClassVM)
        {
            userClassVM.Id = 0;
            var userClass = _mapper.Map<UserClass>(userClassVM);
            await _userClassRepo.AddAsync(userClass);
            TempData["SuccessMessage"] = "User assigned successfully.";

            userClassVM.UserId = 0;
            ViewData["OrgId"] = new SelectList(_organizationRepo.GetTableNoTracking(), "Id", "Name");
            ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking(), "Id", "Name");
            ViewData["UserId"] = new SelectList(await _userManager.Users
                .Include(u => u.UserOrganizations)
                .Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == userClassVM.OrganizationId))
                .ToListAsync(), "Id", "Name");
            ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableNoTracking(), "Id", "Name");
            ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking(), "Id", "Name");
            ViewData["ClassroomId"] = new SelectList(_seasonRepo.GetTableNoTracking(), "Id", "Name");
            ViewData["UserTypeId"] = new SelectList(_userTypeRepo.GetTableNoTracking(), "Id", "Name");
            return View(userClassVM);
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
