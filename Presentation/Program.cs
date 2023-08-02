using Infrastructure.MiddleWares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.Seeder;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Services

#region Connection To SQL Server

builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")!);
}, ServiceLifetime.Transient);

#endregion

#region Dependency injections

builder.Services.AddPersistanceDependencies()
                 .AddInfrastructureDependencies()
                 .AddCoreDependencies()
                 .AddPersistanceServiceRegisteration(builder.Configuration)
                 .AddSerilogRegisteration(builder.Configuration, builder.Host)
                 .AddInfrastructureServiceRegisteration(builder.Configuration);

#endregion

#region MVC

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = ".asp.cookie";
        options.Cookie.Domain = "";
        options.ExpireTimeSpan = TimeSpan.FromDays(2);
    });
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

#endregion

#region API

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region AllowCORS
var CORS = "_cors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CORS,
                      policy =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});

#endregion

#endregion

#region Middlewares

var app = builder.Build();

#region Seed 3 roles and superAdmin user only first time

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    await RoleSeeder.SeedAsync(roleManager);
    await UserSeeder.SeedAsync(userManager);
    await UserTypeSeeder.SeedAsync(dbContext);
}

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(CORS);
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<SchoolAuthorizationMiddleware>();
app.UseCookiePolicy();

app.UseEndpoints(endpoints =>
{
    // API Endpoints using attribute-based routing
    endpoints.MapControllers();

    // MVC Endpoints using conventional routing
    endpoints.MapDefaultControllerRoute();
});
app.Run();

#endregion