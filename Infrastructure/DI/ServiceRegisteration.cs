using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Models.Helpers;
using System.Text;

namespace Infrastructure.DI;

public static class ServiceRegisteration
{
    public static IServiceCollection AddInfrastructureServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
    {
        #region JWT & Cookie Authentication

        var jwtSettings = new JwtSettings();
        configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
        services.AddSingleton(jwtSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "JWT_OR_COOKIE";
            options.DefaultChallengeScheme = "JWT_OR_COOKIE";
            options.DefaultAuthenticateScheme = "JWT_OR_COOKIE";
        })
       .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
       {
           x.RequireHttpsMetadata = false;
           x.SaveToken = true;
           x.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = jwtSettings.ValidateIssuer,
               ValidIssuers = new[] { jwtSettings.Issuer },
               ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
               ValidAudience = jwtSettings.Audience,
               ValidateAudience = jwtSettings.ValidateAudience,
               ValidateLifetime = jwtSettings.ValidateLifeTime,
           };
       })
        .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                bool isApiRequest = context.Request.Path.ToString().Contains("/api/");
                if (isApiRequest)
                    return JwtBearerDefaults.AuthenticationScheme;

                return IdentityConstants.ApplicationScheme;
            };
        });

        #endregion

        #region Swagger Gen
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Schools Management System", Version = "v1"});
            c.EnableAnnotations();

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
            }
           });
        });
        #endregion

        return services;
    }
}
