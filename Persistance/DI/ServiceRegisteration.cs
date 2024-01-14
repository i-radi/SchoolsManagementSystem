using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models.Entities.Identity;
using Models.Helpers;
using System.Security.Claims;

namespace Persistance.DI;

public static class ServiceRegisteration
{
    public static IServiceCollection AddPersistanceServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
    {
        #region Identity
        services.AddIdentity<User, Role>(option =>
        {
            option.SignIn.RequireConfirmedAccount = false;

            // Password settings.
            option.Password.RequireDigit = false;
            option.Password.RequireLowercase = false;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequireUppercase = false;
            option.Password.RequiredLength = 6;
            option.Password.RequiredUniqueChars = 1;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<ApplicationDBContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders()
        .AddSignInManager<CustomSignInManager>();
        #endregion

        return services;
    }
}

public class CustomSignInManager : SignInManager<User>
{
    private readonly UserManager<User> _userManager;
    private readonly IUserRoleRepo _userRoleRepo;

    public CustomSignInManager(UserManager<User> userManager,
        IUserRoleRepo userRoleRepo,
                                IHttpContextAccessor contextAccessor,
                                IUserClaimsPrincipalFactory<User> claimsFactory,
                                IOptions<IdentityOptions> optionsAccessor,
                                IUserConfirmation<User> userConfirmation,
                                IAuthenticationSchemeProvider authenticationSchemeProvider,
                                ILogger<SignInManager<User>> logger)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, authenticationSchemeProvider, userConfirmation)
    {
        _userManager = userManager;
        _userRoleRepo = userRoleRepo;
    }

    public override async Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
    {
        var principal = await base.CreateUserPrincipalAsync(user);

        var roles = (await _userManager.GetRolesAsync(user)).Distinct();

        foreach (var role in roles)
        {
            if (role == "OrganizationAdmin")
            {
                var orgId = _userRoleRepo.GetTableNoTracking().FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == 2)?.OrganizationId;
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(nameof(UserClaimModel.OrgId), orgId.ToString()!));
            }
            if (role == "SchoolAdmin")
            {
                var schoolId = _userRoleRepo.GetTableNoTracking().FirstOrDefault(ur => ur.UserId == user.Id && ur.RoleId == 3)?.SchoolId;
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(nameof(UserClaimModel.SchoolId), schoolId.ToString()!));
            }
        }

        return principal;
    }
}
