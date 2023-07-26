using Microsoft.Extensions.DependencyInjection;
using SMS.Core.IServices;
using SMS.Core.Services;

namespace SMS.Persistance.DI;

public static class ModuleCoreDependencies
{
    public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationService, OrganizationService>();

        return services;
    }
}
