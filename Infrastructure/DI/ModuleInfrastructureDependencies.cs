﻿using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Helpers;

namespace Infrastructure.DI;

public static class ModuleInfrastructureDependencies
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Configuration Of Automapper
        services.AddAutoMapper(AssemblyReference.Assembly);

        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
        services.AddSingleton(emailSettings);
        services.AddTransient<IEmailService, EmailService>();

        //Shared setting 
        var sharedSettings = new SharedSettings();
        configuration.GetSection(nameof(sharedSettings)).Bind(sharedSettings);
        services.AddSingleton(sharedSettings);

        services.AddScoped<IExportService<UserViewModel>, ExportService<UserViewModel>>();
        services.AddScoped<IExportService<GetUserDto>, ExportService<GetUserDto>>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<ICsvService, CsvService>();
        services.AddScoped<IHtmlService, HtmlService>();
        services.AddScoped<IJsonService, JsonService>();
        services.AddScoped<IXmlService, XmlService>();
        services.AddScoped<IYamlService, YamlService>();
        services.AddScoped<IAttachmentService, AttachmentService>();


        // Get Validators
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddControllersWithViews()
            .ConfigureApiBehaviorOptions(options =>
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

        var baseSettings = new BaseSettings();
        configuration.GetSection(nameof(baseSettings)).Bind(baseSettings);
        services.AddSingleton(baseSettings);

        return services;
    }
}
