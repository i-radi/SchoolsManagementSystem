using Presentation.Controllers.MVC;

namespace Presentation.DI.MVC;

public record UsersControllerDI(
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
    IAttachmentService attachmentService);
