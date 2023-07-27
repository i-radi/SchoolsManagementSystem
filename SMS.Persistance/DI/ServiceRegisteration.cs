using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SMS.Models.Entities.Identity;

namespace SMS.Persistance.DI;

public static class ServiceRegisteration
{
    public static IServiceCollection AddPersistanceServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
    {
        #region Identity
        services.AddIdentity<User, Role>(option =>
        {
            // Password settings.
            option.Password.RequireDigit = false;
            option.Password.RequireLowercase = false;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequireUppercase = false;
            option.Password.RequiredLength = 6;
            option.Password.RequiredUniqueChars = 1;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;

        }).AddEntityFrameworkStores<ApplicationDBContext>();
        #endregion

        return services;
    }

}
