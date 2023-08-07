using Infrastructure.IServices;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Helpers;
using System.Reflection;

namespace Infrastructure.DI;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration Of Automapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
        services.AddSingleton(emailSettings);
        services.AddTransient<IEmailSender, EmailSender>();

        // Get Validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddControllersWithViews().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = c =>
            {
                var errors = string.Join('\n', c.ModelState.Values.Where(v => v.Errors.Count > 0)
                  .SelectMany(v => v.Errors)
                  .Select(v => v.ErrorMessage));

                throw new ValidationException(errors);
            };
        });
        services.AddRazorPages();
        services.AddMvc();
        return services;
    }
}
