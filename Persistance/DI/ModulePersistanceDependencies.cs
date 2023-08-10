using Microsoft.Extensions.DependencyInjection;
using Persistance.Repos;

namespace Persistance.DI;

public static class ModulePersistanceDependencies
{
    public static IServiceCollection AddPersistanceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClassRoomRepo, ClassRoomRepo>();
        services.AddScoped<IGradeRepo, GradeRepo>();
        services.AddScoped<IOrganizationRepo, OrganizationRepo>();
        services.AddScoped<ISchoolRepo, SchoolRepo>();
        services.AddScoped<ISeasonRepo, SeasonRepo>();
        services.AddScoped<IUserClassRepo, UserClassRepo>();
        services.AddScoped<IUserTypeRepo, UserTypeRepo>();
        services.AddScoped<IActivityRepo, ActivityRepo>();
        services.AddScoped(typeof(IGenericRepoAsync<>), typeof(GenericRepoAsync<>));
        return services;
    }
}
