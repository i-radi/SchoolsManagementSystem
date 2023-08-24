using Infrastructure.MiddleWares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace Infrastructure.DI;

public static class SerilogRegisteration
{
    public static IServiceCollection AddSerilogRegisteration(this IServiceCollection services, IConfiguration configuration, IHostBuilder host)
    {

        #region Senk Email
        var connectionString = configuration.GetConnectionString("ApplicationDbContextConnection");


#pragma warning disable CS0618 // Type or member is obsolete
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.MSSqlServer(connectionString, "Logs",
                columnOptions: GetMSSqlServerColumnOptions(),
                autoCreateSqlTable: true)
            .WriteTo.Console()
            .WriteTo.File($"Logs/{DateTime.Now.ToString("MMMM-yyyy")}/{DateTime.Now.ToString("dd-MM-yyyy")}.txt", LogEventLevel.Information);
#pragma warning restore CS0618 // Type or member is obsolete

        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

        if (emailSettings.SendEmails)
        {
            loggerConfiguration = loggerConfiguration
                    .WriteTo.Email(new EmailConnectionInfo
                    {
                        FromEmail = emailSettings.FromEmail,
                        ToEmail = emailSettings.ToEmails,
                        MailServer = emailSettings.SmtpServer,
                        EnableSsl = emailSettings.EnableSsl,
                        Port = emailSettings.Port,
                        NetworkCredentials = new System.Net.NetworkCredential
                        {
                            UserName = emailSettings.UserName,
                            Password = emailSettings.Password
                        },
                        IsBodyHtml = false,
                        EmailSubject = "Error Logs SMS",

                    }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error);
        }
        loggerConfiguration = loggerConfiguration.ReadFrom.Configuration(configuration);

        Log.Logger = loggerConfiguration.CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });


        #endregion

        services.AddSingleton<RequestResponseLoggingMiddleware>();

        return services;
    }

    [Obsolete]
    private static ColumnOptions GetMSSqlServerColumnOptions()
    {
        var columnOptions = new ColumnOptions
        {
            AdditionalDataColumns = new List<DataColumn>
            {
                new DataColumn {DataType = typeof(string), ColumnName = "UserIpAddress", AllowDBNull = true}
            }
        };

        return columnOptions;
    }


}
