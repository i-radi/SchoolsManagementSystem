using Infrastructure.MiddleWares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;

namespace Infrastructure.DI;

public static class SerilogRegisteration
{
    public static IServiceCollection AddSerilogRegisteration(this IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {

        #region Senk Email

        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

        if (emailSettings.SendEmails)
        {
            host.UseSerilog((context, configuration) =>
            configuration
             .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("log.json", LogEventLevel.Information)
                    .WriteTo.Email(new EmailConnectionInfo
                    {
                        FromEmail = emailSettings.FromEmail,
                        ToEmail = emailSettings.ToEmails,
                        MailServer = emailSettings.SmtpServer,
                        EmailSubject = emailSettings.EmailSubject,
                        EnableSsl = emailSettings.EnableSsl,
                        Port = emailSettings.Port, // Use the appropriate SMTP port (e.g., 465 for gmail)
                        NetworkCredentials = new System.Net.NetworkCredential
                        {
                            UserName = emailSettings.UserName, //your SMTP server username (if required)
                            Password = emailSettings.Password //your SMTP server password (if required)
                        }
                    }, restrictedToMinimumLevel: LogEventLevel.Error,
                    batchPostingLimit: 1,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"));
        }
        else
        {
            host.UseSerilog((context, configuration) =>
            configuration
             .MinimumLevel.Information()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("log.json", LogEventLevel.Information));
        }

        #endregion

        services.AddSingleton<RequestResponseLoggingMiddleware>();

        return services;
    }
}
