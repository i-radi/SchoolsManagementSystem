using Microsoft.EntityFrameworkCore;
using Models.Entities.Identity;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg;
using System.Drawing.Printing;
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
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var userclass = _userClassRepo.GetTableNoTracking()
                .Include(u => u.User)
                .Include(u => u.Classroom)
                .Include(u => u.Season)
                .Include(u => u.UserType)
                .AsQueryable();


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

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.Value);
            if (user == null)
            {
                return NotFound();
            }

            var userVM = _mapper.Map<UserViewModel>(user);
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
        public async Task<IActionResult> Assign(int? userId, int? orgid, int? schoolid, int? gradeid)
        {

            IQueryable<User> usersQuery = _userManager.Users
                .Include(u => u.UserOrganizations)
                .ThenInclude(uo => uo.Organization)
                .AsQueryable();

            if (userId is not null)
            {
                usersQuery = usersQuery.Where(u => u.Id == userId);
            }
            else
            {
                if (orgid is not null)
                {
                    usersQuery = usersQuery.Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == orgid));
                }
            }


            if (usersQuery == null)
            {
                return NotFound();
            }
            var organizations = _organizationRepo.GetTableNoTracking().ToList();
            var schools = _schoolRepo.GetTableNoTracking().ToList();
            var grades = _gradeRepo.GetTableNoTracking().ToList();
            var classrooms = _classroomRepo.GetTableNoTracking().Include(c => c.Grade).ToList();
            var seasons = _seasonRepo.GetTableNoTracking().ToList();
            var usertypes = _userTypeRepo.GetTableNoTracking().ToList();

            ViewData["UserId"] = new SelectList(usersQuery, "Id", "Name");
            ViewData["OrgId"] = new SelectList(organizations, "Id", "Name");
            ViewData["SchoolId"] = new SelectList(schools, "Id", "Name");
            ViewData["GradeId"] = new SelectList(grades, "Id", "Name");
            ViewData["ClassroomId"] = new SelectList(classrooms, "Id", "Name");
            ViewData["UserTypeId"] = new SelectList(usertypes, "Id", "Name");
            ViewData["SeasonId"] = new SelectList(seasons, "Id", "Name");
            return View(new UserClassViewModel { UserId = usersQuery.FirstOrDefault().Id });



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
