using Core.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Persistance.DI;

public static class ModuleCoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClassroomService, ClassroomService>();
        services.AddScoped<IGradeService, GradeService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<ISchoolService, SchoolService>();
        services.AddScoped<ISeasonService, SeasonService>();
        services.AddScoped<IUserClassService, UserClassService>();
        services.AddScoped<IUserTypeService, UserTypeService>();
        services.AddScoped<IActivityService, ActivityService>();
        services.AddScoped<IActivityClassroomService, ActivityClassroomService>();
        services.AddScoped<IActivityInstanceService, ActivityInstanceService>();
        services.AddScoped<IActivityInstanceUserService, ActivityInstanceUserService>();
        services.AddScoped<IActivityTimeService, ActivityTimeService>();

        #region Authorization

        services.AddAuthorization(options =>
        {
            options.AddPolicy("SuperAdmin",
                policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin"));
            options.AddPolicy("OrganizationAdmin",
                policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin", "OrganizationAdmin"));
            options.AddPolicy("SchoolAdmin",
                policy => policy.RequireClaim(ClaimTypes.Role, "SuperAdmin", "OrganizationAdmin", "SchoolAdmin"));

        }
        );

        #endregion

        return services;
    }
}
