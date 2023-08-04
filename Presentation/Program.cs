using Infrastructure.MiddleWares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.Seeder;
using Serilog;
using Persistance.Context;

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
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging();
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(CORS);


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<SchoolAuthorizationMiddleware>();

app.UseEndpoints(endpoints =>
{
    // API Endpoints using attribute-based routing
    endpoints.MapControllers();

    // MVC Endpoints using conventional routing
    endpoints.MapDefaultControllerRoute();
});
app.MapRazorPages();

app.Run();

#endregion