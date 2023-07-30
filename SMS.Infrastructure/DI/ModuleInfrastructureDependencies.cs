using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SMS.Infrastructure.DI;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        //Configuration Of Automapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Get Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = c =>
            {
                var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                  .SelectMany(v => v.Errors)
                  .Select(v => v.ErrorMessage));

                throw new ValidationException(errors);
            };
        });

        return services;
    }
}
