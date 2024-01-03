using NuGet.Packaging;
using OfficeOpenXml;

namespace Presentation.Controllers.MVC;

[Authorize(Policy = "SuperAdmin")]
public class UsersController(
    ILogger<UsersController> logger,
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    IOrganizationRepo organizationRepo,
    IUserOrganizationRepo userOrganizationRepo,
    IUserRoleRepo userRoleRepo,
    IUserClassRepo userClassRepo,
    ISchoolRepo schoolRepo,
    IActivityRepo activityRepo,
    ISeasonRepo seasonRepo,
    IGradeRepo gradeRepo,
    IClassroomRepo classroomRepo,
    IUserTypeRepo userTypeRepo,
    IAuthService authService,
    IMapper mapper,
    ApplicationDBContext context,
    IWebHostEnvironment webHostEnvironment,
    BaseSettings baseSettings,
    IAttachmentService attachmentService,
    UserSettings userSettings) : Controller
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<Role> _roleManager = roleManager;
    private readonly IOrganizationRepo _organizationRepo = organizationRepo;
    private readonly IUserOrganizationRepo _userOrganizationRepo = userOrganizationRepo;
    private readonly IUserRoleRepo _userRoleRepo = userRoleRepo;
    private readonly IUserClassRepo _userClassRepo = userClassRepo;
    private readonly ISchoolRepo _schoolRepo = schoolRepo;
    private readonly IActivityRepo _activityRepo = activityRepo;
    private readonly ISeasonRepo _seasonRepo = seasonRepo;
    private readonly IGradeRepo _gradeRepo = gradeRepo;
    private readonly IClassroomRepo _classroomRepo = classroomRepo;
    private readonly IUserTypeRepo _userTypeRepo = userTypeRepo;
    private readonly IAuthService _authService = authService;
    private readonly IMapper _mapper = mapper;
    private readonly ApplicationDBContext _context = context;
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    private readonly BaseSettings _baseSettings = baseSettings;
    private readonly IAttachmentService _attachmentService = attachmentService;
    private readonly UserSettings _userSettings = userSettings;
    

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

    public IActionResult Create()
    {
        ViewData["SchoolId"] = new SelectList(_schoolRepo.GetTableNoTracking().ToList(), "Id", "Name");
        ViewData["OrganizationId"] = new SelectList(_organizationRepo.GetTableNoTracking().ToList(), "Id", "Name");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserFormViewModel user)
    {
        // any text read or write must be Parameters 
        string CreatedEmail = (string.IsNullOrEmpty(user.Email)) ? Guid.NewGuid() + _userSettings.Suffix : user.Email;
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

            newUser.ProfilePicturePath = await _attachmentService.Upload(
                user.ProfilePicture,
                _webHostEnvironment,
                _baseSettings.usersPath,
                $"{newUser.UserName}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
        }
        else
        {
            newUser.ProfilePicturePath = "";
        }

        var result = await _userManager.CreateAsync(newUser, newUser.PlainPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
        createdUser!.ParticipationNumber = createdUser.Id;
        createdUser!.ParticipationQRCodePath = _attachmentService.GenerateQrCode(createdUser.Id, _webHostEnvironment);
        _context.User.Update(createdUser);
        await _context.SaveChangesAsync();

        foreach (var selectedOrgId in user.SelectedOrganizationIds)
        {
            var userOrg = new UserOrganization
            {
                UserId = createdUser.Id,
                OrganizationId = selectedOrgId
            };
            await _userOrganizationRepo.AddAsync(userOrg);
        }

        return RedirectToAction(nameof(Index), "Users");
    }

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserFormViewModel userVM)
    {
        if (id != userVM.Id)
        {
            return NotFound();
        }
        userVM.Email = (string.IsNullOrEmpty(userVM.Email)) ? Guid.NewGuid() + _userSettings.Suffix : userVM.Email;
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
                var fileExtension = Path.GetExtension(Path.GetFileName(userVM.ProfilePicture.FileName));

                updatedUser.ProfilePicturePath = await _attachmentService.Upload(
                    userVM.ProfilePicture,
                    _webHostEnvironment,
                    _baseSettings.usersPath,
                    $"{updatedUser.UserName}-{DateTime.Now.ToShortDateString().Replace('/', '_')}{fileExtension}");
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
            .Select(o => new { ActivityId = o.Id, o.Name }).ToListAsync();

        var createRoleViewModel = new CreateRoleViewModel
        {
            Id = userId,
            UserName = user!.Email!,
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

    public async Task<IActionResult> AddBulk()
    {
        ViewBag.OrganizationsList = new SelectList(await _organizationRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SchoolsList = new SelectList(await _schoolRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SeasonsList = new SelectList(await _seasonRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.GradesList = new SelectList(await _gradeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.ClassroomsList = new SelectList(await _classroomRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.UserTypesList = new SelectList(await _userTypeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        return View(new BulkUserClassViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBulk(BulkUserClassViewModel viewModel)
    {
        ViewBag.OrganizationsList = new SelectList(await _organizationRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SchoolsList = new SelectList(await _schoolRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.SeasonsList = new SelectList(await _seasonRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.GradesList = new SelectList(await _gradeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.ClassroomsList = new SelectList(await _classroomRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        ViewBag.UserTypesList = new SelectList(await _userTypeRepo.GetTableNoTracking().ToListAsync(), "Id", "Name");
        try
        {
            var result = new Result<List<User>>();
            var allUsers = new List<UserFormViewModel>();

            if (viewModel.Attachment is null || viewModel.Attachment.Length < 1)
            {
                result.Succeeded = false;
                result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                result.Errors!.Add("Empty File!!");
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                return View(viewModel);
            }

            Stream stream = viewModel.Attachment!.OpenReadStream();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (ExcelPackage package = new(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.TrimLastEmptyRows();
                int colCount = worksheet.Dimension.Columns;
                int rowCount = worksheet.Dimension.Rows;
                if (rowCount < 2)
                {
                    result.Succeeded = false;
                    result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    ModelState.AddModelError(string.Empty, "No rows Added!!");
                    return View(viewModel);
                }
                var validationResult = ValidateSheetAsync(worksheet);

                if (!validationResult.isValid)
                {
                    result.Succeeded = false;
                    result.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    var errors = validationResult.errors?.ToList();
                    result.Errors = errors!;
                    ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors));
                    return View(viewModel);
                }
                allUsers = validationResult.usersInfos;
            }

            result = await AddUsersInDataBaseAsync(allUsers!);
            if (!(result.StatusCode == System.Net.HttpStatusCode.OK
                || result.StatusCode == System.Net.HttpStatusCode.MultiStatus))
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", result.Errors!));
                return View(viewModel);
            }

            if (IsAssignToOrganization(viewModel))
            {
                await AssignOrganization(result.Data!, viewModel.OrganizationId);
            }

            if (IsAssignToClassroom(viewModel))
            {
                await AssignClassroom(result.Data!, viewModel);
            }

            TempData["SuccessMessage"] = "Users added " +
                (IsAssignToOrganization(viewModel) ? ", assign to organization " : "") +
                (IsAssignToClassroom(viewModel) ? "and assign to classroom " : "") +
                "successfully.";

            return RedirectToAction();
        }
        catch (Exception e)
        {
            ModelState.AddModelError(string.Empty, e.Message);
            return View(viewModel);
        }
    }

    private (bool isValid, List<UserFormViewModel>? usersInfos, List<string>? errors) ValidateSheetAsync(ExcelWorksheet worksheet)
    {
        var users = new List<UserFormViewModel>();
        int colCount = worksheet.Dimension.Columns;
        int rowCount = worksheet.Dimension.Rows;
        List<string> allErrors = new();
        _ = new List<string>();

        if (colCount > 1001)
        {
            allErrors.Add("The maximum number of users to upload is 1000 users per sheet.");
        }
        
        for (int row = 2; row <= rowCount; row++)
        {
            UserFormViewModel user = new();
            if (worksheet.Cells[row, 1].Value != null)
            {
                if (Check.IsEmail(worksheet.Cells[row, 1].Value?.ToString()?.Trim() ?? ""))
                {
                    user.Email = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                }
                else
                {
                    allErrors.Add($"Wrong email format in row ({row}).");
                }
            }
            else
            {

                user.Email = Guid.NewGuid() + _userSettings.Suffix; 
            }
            if (worksheet.Cells[row, 2].Value != null)
            {
                if (Check.InLength(worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? ""))
                {
                    user.Name = worksheet.Cells[row, 2].Value!.ToString()!.Trim();
                }
                else
                {
                    allErrors.Add($"You can only use letters, numbers between 3 to 50 characters in row ({row}).");
                }
            }
            else
            {
                allErrors.Add($"Required [Name] field is missing in row ({row})");
            }
            user.Address = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
            user.Gender = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
            if (worksheet.Cells[row, 5].Value != null)
            {
                if (DateTime.TryParse(worksheet.Cells[row, 5].Value?.ToString()?.Trim(), out DateTime birthdate))
                {
                    user.Birthdate = birthdate;
                }
                else
                {
                    allErrors.Add($"Invalid date in row ({row})");
                }
            }
            user.SchoolUniversityJob = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
            user.GpsLocation = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
            user.NationalID = worksheet.Cells[row, 8].Value?.ToString()?.Trim();
            user.MentorName = worksheet.Cells[row, 9].Value?.ToString()?.Trim();
            user.FirstMobile = worksheet.Cells[row, 10].Value?.ToString()?.Trim();
            user.SecondMobile = worksheet.Cells[row, 11].Value?.ToString()?.Trim();
            user.FatherMobile = worksheet.Cells[row, 12].Value?.ToString()?.Trim();
            user.MotherMobile = worksheet.Cells[row, 13].Value?.ToString()?.Trim();

            users.Add(user);
        }

        if (allErrors.Count > 0)
        {
            return (isValid: false, usersInfos: null, errors: allErrors);
        }

        return (isValid: true, usersInfos: users, errors: null);
    }

    private async Task<Result<List<User>>> AddUsersInDataBaseAsync(List<UserFormViewModel> users)
    {
        Result<List<User>> result = new()
        {
            Data = new List<User>()
        };
        foreach (var user in users)
        {
            var userResult = await AddUserAsync(user);
            if (!userResult.Succeeded)
            {
                result.Succeeded = userResult.Succeeded;
                result.Errors = userResult.Errors;
                result.StatusCode = userResult.StatusCode;
                return result;
            }
            result.Data.Add(userResult.Data!);
        }

        result.Succeeded = true;
        result.StatusCode = System.Net.HttpStatusCode.OK;
        return result;
    }

    private async Task<Result<User>> AddUserAsync(UserFormViewModel user)
    {
        Result<User> resultUser = new();

        string CreatedEmail = (string.IsNullOrEmpty(user.Email)) ? Guid.NewGuid() + _userSettings.Suffix : user.Email;
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
            NationalID = user.NationalID,
            ProfilePicturePath = _userSettings.DefaultImage 
        };

        var result = await _userManager.CreateAsync(newUser, newUser.PlainPassword);
        if (!result.Succeeded)
        {
            resultUser.Succeeded = false;
            resultUser.StatusCode = System.Net.HttpStatusCode.BadRequest;
            resultUser.Errors = result.Errors.Select(e => $"{e.Code} : {e.Description}").ToList();
            return resultUser;
        }

        var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
        createdUser!.ParticipationNumber = createdUser.Id;
        createdUser!.ParticipationQRCodePath = _attachmentService.GenerateQrCode(createdUser.Id, _webHostEnvironment);
        _context.User.Update(createdUser);
        await _context.SaveChangesAsync();

        resultUser.Succeeded = true;
        resultUser.StatusCode = System.Net.HttpStatusCode.OK;
        resultUser.Data = createdUser;
        return resultUser;
    }

    private async Task<bool> AssignOrganization(List<User> users, int organizationId)
    {
        foreach (var user in users)
        {
            var userOrg = new UserOrganization
            {
                UserId = user.Id,
                OrganizationId = organizationId
            };
            await _userOrganizationRepo.AddAsync(userOrg);
        }
        return true;
    }

    private static bool IsAssignToOrganization(BulkUserClassViewModel viewModel)
    {
        return viewModel.OrganizationId > 0;
    }

    private async Task<bool> AssignClassroom(List<User> users, BulkUserClassViewModel viewmodel)
    {
        foreach (var user in users)
        {
            var userClass = new UserClass
            {
                UserId = user.Id,
                ClassroomId = viewmodel.ClassroomId,
                SeasonId = viewmodel.SeasonId,
                UserTypeId = viewmodel.UserTypeId,
            };
            await _userClassRepo.AddAsync(userClass);
        }
        return true;
    }

    private static bool IsAssignToClassroom(BulkUserClassViewModel viewModel) => viewModel.ClassroomId > 0 && viewModel.SeasonId > 0 && viewModel.UserTypeId > 0;

    public IActionResult DownloadExcelSheet()
    {
        var templateFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "files", "Multiple Users Template.xlsx");

        ExcelPackage.LicenseContext = LicenseContext.Commercial;
        using (var package = new ExcelPackage(new FileInfo(templateFilePath)))
        {
            var stream = new MemoryStream(package.GetAsByteArray());

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Headers.Add("content-disposition", "attachment; filename=Multiple Users Template.xlsx");

            stream.CopyTo(Response.Body);
        }
        return new EmptyResult();
    }

    [HttpGet]
    public async Task<IActionResult> GetSchoolsByOrganization(int organizationId)
    {
        List<School> schools = await _schoolRepo
            .GetTableNoTracking()
            .Where(s => s.OrganizationId == organizationId)
            .ToListAsync();

        var schoolList = schools.Select(school => new SelectListItem
        {
            Value = school.Id.ToString(),
            Text = school.Name
        }).ToList();

        return Json(schoolList);
    }

    [HttpGet]
    public async Task<IActionResult> GetSeasonsBySchool(int schoolId)
    {
        var seasons = await _seasonRepo
            .GetTableNoTracking()
            .Where(s => s.SchoolId == schoolId)
            .ToListAsync();

        var seasonsList = seasons.Select(season => new SelectListItem
        {
            Value = season.Id.ToString(),
            Text = season.Name
        }).ToList();

        return Json(seasonsList);
    }

    [HttpGet]
    public async Task<IActionResult> GetGradesBySchool(int schoolId)
    {
        var grades = await _gradeRepo
            .GetTableNoTracking()
            .Where(s => s.SchoolId == schoolId)
            .ToListAsync();

        var gradeList = grades.Select(grade => new SelectListItem
        {
            Value = grade.Id.ToString(),
            Text = grade.Name
        }).ToList();

        return Json(gradeList);
    }

    [HttpGet]
    public async Task<IActionResult> GetClassroomsByGrade(int gradeId)
    {
        var grades = await _classroomRepo
            .GetTableNoTracking()
            .Where(s => s.GradeId == gradeId)
            .ToListAsync();

        var gradeList = grades.Select(grade => new SelectListItem
        {
            Value = grade.Id.ToString(),
            Text = grade.Name
        }).ToList();

        return Json(gradeList);
    }
}
