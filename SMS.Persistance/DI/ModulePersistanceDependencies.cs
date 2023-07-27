using Microsoft.Extensions.DependencyInjection;
using SMS.Persistance.Repos;

namespace SMS.Persistance.DI;

public static class ModulePersistanceDependencies
{
    public static IServiceCollection AddPersistanceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClassesRepo, ClassesRepo>();
        services.AddScoped<IGradeRepo, GradeRepo>();
        services.AddScoped<IOrganizationRepo, OrganizationRepo>();
        services.AddScoped<ISchoolRepo, SchoolRepo>();
        services.AddScoped<ISeasonRepo, SeasonRepo>();
        services.AddScoped(typeof(IGenericRepoAsync<>), typeof(GenericRepoAsync<>));
        return services;
    }
}
