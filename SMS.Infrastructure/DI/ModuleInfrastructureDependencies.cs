using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SMS.Infrastructure.DI;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        //Configuration Of Automapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}
