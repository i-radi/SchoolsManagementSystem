

namespace SMS.Tests;

public static class ConfigurationMock
{
    public static IConfiguration GetConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return config;
    }
}