using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.Entities.Identity;

namespace Persistance.DI;

public static class ServiceRegisteration
{
    public static IServiceCollection AddPersistanceServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
    {
        #region Identity
        services.AddIdentity<User, Role>(option =>
        {
            option.SignIn.RequireConfirmedAccount = false;

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

        }).AddEntityFrameworkStores<ApplicationDBContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders()
        .AddSignInManager<SignInManager<User>>();

        services.Configure<SecurityStampValidatorOptions>(options =>
                                                            options.ValidationInterval = TimeSpan.Zero);
        #endregion

        return services;
    }

}
