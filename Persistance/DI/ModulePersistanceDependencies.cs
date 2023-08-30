using Microsoft.Extensions.DependencyInjection;
using Persistance.Repos;

namespace Persistance.DI;

public static class ModulePersistanceDependencies
{
    public static IServiceCollection AddPersistanceDependencies(this IServiceCollection services)
    {
        services.AddScoped<IClassroomRepo, ClassroomRepo>();
        services.AddScoped<IGradeRepo, GradeRepo>();
        services.AddScoped<IOrganizationRepo, OrganizationRepo>();
        services.AddScoped<IUserOrganizationRepo, UserOrganizationRepo>();
        services.AddScoped<ISchoolRepo, SchoolRepo>();
        services.AddScoped<ISeasonRepo, SeasonRepo>();
        services.AddScoped<IUserClassRepo, UserClassRepo>();
        services.AddScoped<IUserRoleRepo, UserRoleRepo>();
        services.AddScoped<IUserTypeRepo, UserTypeRepo>();
        services.AddScoped<IActivityRepo, ActivityRepo>();
        services.AddScoped<IActivityTimeRepo, ActivityTimeRepo>();
        services.AddScoped<IActivityClassroomRepo, ActivityClassroomRepo>();
        services.AddScoped<IActivityInstanceRepo, ActivityInstanceRepo>();
        services.AddScoped<IActivityInstanceUserRepo, ActivityInstanceUserRepo>();
        services.AddScoped(typeof(IGenericRepoAsync<>), typeof(GenericRepoAsync<>));
        return services;
    }
}
