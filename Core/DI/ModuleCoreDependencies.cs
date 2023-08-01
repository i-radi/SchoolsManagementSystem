using Microsoft.Extensions.DependencyInjection;
using Core.Services;
using System.Security.Claims;

namespace Persistance.DI;

public static class ModuleCoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClassRoomService, ClassRoomService>();
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<ISchoolService, SchoolService>();
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<IUserClassService, UserClassService>();
        services.AddScoped<IUserTypeService, UserTypeService>();

        #region Authorization

        services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdmin",
                policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin"));
            options.AddPolicy("Admin",
                policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin", "Admin"));
            options.AddPolicy("Normal",
                policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin", "Admin", "Normal"));

        }
        );

        #endregion

        return services;
    }
}
