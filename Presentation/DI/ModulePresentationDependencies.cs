using Core.Services;
using Presentation.DI.MVC;

namespace Presentation.DI;

public static class ModulePresentationDependencies
{
    public static IServiceCollection AddPresentationDependencies(this IServiceCollection services)
    {
        services.AddScoped<UsersControllerDI>();

        return services;
    }
}

