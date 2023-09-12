using Newtonsoft.Json;
using System.Linq.Dynamic.Core;

namespace Presentation.Controllers.MVC;

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
    public async Task<IActionResult> Index(
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
            .AsSplitQuery()
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

        ViewBag.OrganizationsList = new SelectList(await _organizationRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SchoolsList = new SelectList(await _schoolRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SeasonsList = new SelectList(await _seasonRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.GradesList = new SelectList(await _gradeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.ClassroomsList = new SelectList(await _classroomRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.UserTypesList = new SelectList(await _userTypeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");

        var result = PaginatedList<UserClassViewModel>.Create(_mapper.Map<List<UserClassViewModel>>(await userclass.ToListAsync()), pageNumber, pageSize);
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

    // GET: Members/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var modelItem = await _userClassRepo
            .GetTableAsTracking()
            .Include(uc => uc.User)
            .Include(uc => uc.Season)
            .ThenInclude(s => s!.School)
            .ThenInclude(sc => sc!.Organization)
            .Include(uc => uc.Classroom)
            .ThenInclude(c => c.Grade)
            .Include(uc => uc.UserType)
            .FirstOrDefaultAsync(uc => uc.Id == id);

        if (modelItem == null)
        {
            return NotFound();
        }
        var viewModel = new UserClassViewModel
        {
            Id = id.Value,
            OrganizationId = modelItem.Season!.School!.OrganizationId,
            SchoolId = modelItem.Season.SchoolId,
            SeasonId = modelItem.SeasonId,
            GradeId = modelItem.Classroom!.GradeId,
            UserId = modelItem.UserId,
            ClassroomId = modelItem.ClassroomId,
            UserTypeId = modelItem.UserTypeId
        };

        ViewData["OrgId"] = new SelectList(_organizationRepo.GetTableNoTracking(), "Id", "Name");
        ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking(), "Id", "Name");
        ViewData["UserId"] = new SelectList(await _userManager.Users
            .Include(u => u.UserOrganizations)
            .Where(u => u.UserOrganizations.Any(uo => uo.OrganizationId == viewModel.OrganizationId))
            .ToListAsync(), "Id", "Name");
        ViewData["GradeId"] = new SelectList(_gradeRepo.GetTableNoTracking(), "Id", "Name");
        ViewData["SeasonId"] = new SelectList(_seasonRepo.GetTableNoTracking(), "Id", "Name");
        ViewData["ClassroomId"] = new SelectList(_seasonRepo.GetTableNoTracking(), "Id", "Name");
        ViewData["UserTypeId"] = new SelectList(_userTypeRepo.GetTableNoTracking(), "Id", "Name");
        return View(viewModel);
    }

    // POST: Members/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserClassViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }
        var updatedUserClass = await _userClassRepo.GetByIdAsync(id);
        if (updatedUserClass is not null)
        {
            updatedUserClass.UserId = viewModel.UserId;
            updatedUserClass.SeasonId = viewModel.SeasonId;
            updatedUserClass.ClassroomId = viewModel.ClassroomId;
            updatedUserClass.UserTypeId = viewModel.UserTypeId;

            try
            {
                await _userClassRepo.UpdateAsync(updatedUserClass);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        return View(viewModel);
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
        ViewData["ClassroomId"] = new SelectList(_classroomRepo.GetTableNoTracking(), "Id", "Name");
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

    public async Task<IActionResult> Copy(
        int pageNumber = 1,
        int pageSize = 10,
        int fromOrgId = 0,
        int fromSchoolId = 0,
        int fromSeasonId = 0,
        int fromGradeId = 0,
        int fromClassroomId = 0,
        int fdromUsertypeId = 0,
        string searchUserName = "")
    {
        var userclass = _userClassRepo.GetTableNoTracking()
            .Include(u => u.User)
            .Include(u => u.Classroom)
            .Include(u => u.Season)
            .ThenInclude(s => s.School)
            .Include(u => u.UserType)
            .AsSplitQuery()
            .AsQueryable();

        if (fromOrgId != 0)
        {
            userclass = userclass.Where(uc => uc.Season!.School!.OrganizationId == fromOrgId);
        }
        if (fromSchoolId != 0)
        {
            userclass = userclass.Where(uc => uc.Season!.SchoolId == fromSchoolId);
        }
        if (fromSeasonId != 0)
        {
            userclass = userclass.Where(uc => uc.SeasonId == fromSeasonId);
        }
        if (fromGradeId != 0)
        {
            userclass = userclass.Where(uc => uc.Classroom!.GradeId == fromGradeId);
        }
        if (fromClassroomId != 0)
        {
            userclass = userclass.Where(uc => uc.ClassroomId == fromClassroomId);
        }
        if (fdromUsertypeId != 0)
        {
            userclass = userclass.Where(uc => uc.UserTypeId == fdromUsertypeId);
        }
        if (!string.IsNullOrWhiteSpace(searchUserName))
        {
            userclass = userclass.Where(uc => uc.User!.Email!.ToLower().Contains(searchUserName.ToLower())
            || uc.User!.Name!.ToLower().Contains(searchUserName.ToLower()));
        }

        ViewBag.OrganizationsList = new SelectList(await _organizationRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SchoolsList = new SelectList(await _schoolRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SeasonsList = new SelectList(await _seasonRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.GradesList = new SelectList(await _gradeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.ClassroomsList = new SelectList(await _classroomRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.UserTypesList = new SelectList(await _userTypeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");

        var fromClassMembers = PaginatedList<UserClassViewModel>.Create(_mapper.Map<List<UserClassViewModel>>(await userclass.ToListAsync()), pageNumber, pageSize);

        if (fromClassroomId != 0)
        {
            return View(new CopyUserClassViewModel
            {
                From = fromClassMembers
            });
        }
        return View(new CopyUserClassViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Copy(CopyUserClassViewModel model)
    {
        if (ModelState.IsValid)
        {
            var selectedUserIds = model.SelectedUserIds.Split(',')
                                            .Select(id => int.Parse(id))
                                            .ToList();


            foreach (var userId in selectedUserIds)
            {
                var userClass = await _userClassRepo
                    .GetTableNoTracking()
                    .FirstOrDefaultAsync(uc => uc.UserId == userId
                    && uc.ClassroomId == model.FromClassroomId
                    && uc.SeasonId == model.FromSeasonId);

                if (userClass != null)
                {
                    userClass.ClassroomId = model.ToClassroomId;
                    userClass.SeasonId = model.ToSeasonId;

                    await _userClassRepo.UpdateAsync(userClass);
                }
            }
            return RedirectToAction("Copy");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Please select users to copy.");
        }
        return View("Copy", model);
    }

}
