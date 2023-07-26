using Microsoft.Extensions.DependencyInjection;
using SMS.Core.IServices;
using SMS.Core.Services;
using System.Security.Claims;

namespace SMS.Persistance.DI;

public static class ModuleCoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IAuthService, AuthService>();

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
