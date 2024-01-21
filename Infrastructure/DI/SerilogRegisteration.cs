using Infrastructure.MiddleWares;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Models.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace Infrastructure.DI;

public static class SerilogRegisteration
{
    [Obsolete]
    public static IServiceCollection AddSerilogRegisteration(this IServiceCollection services, IConfiguration configuration)
    {

        var connectionString = configuration.GetConnectionString("ApplicationDbContextConnection");


        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.MSSqlServer(connectionString, "Logs",
                columnOptions: GetMSSqlServerColumnOptions(),
                autoCreateSqlTable: true)
            .WriteTo.Console()
            .WriteTo.File($"Logs/{DateTime.Now.ToString("MMMM-yyyy")}/{DateTime.Now.ToString("dd-MM-yyyy")}.txt", LogEventLevel.Information);

        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);

        loggerConfiguration = loggerConfiguration.ReadFrom.Configuration(configuration);

        Log.Logger = loggerConfiguration.CreateLogger();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });



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
