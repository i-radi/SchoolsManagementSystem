using Microsoft.Extensions.DependencyInjection;
using SMS.Core.Services;
using System.Security.Claims;

namespace SMS.Persistance.DI;

public static class ModuleCoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClassesService, ClassesService>();
        services.AddScoped<IGradeService, GradesService>();
        services.AddScoped<IOrganizationService, OrganizationsService>();
        services.AddScoped<ISchoolService, SchoolsService>();
        services.AddScoped<ISeasonService, SeasonsService>();
        services.AddScoped<IUserClassService, UserClassesService>();
        services.AddScoped<IUserTypeService, UserTypesService>();

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
