using Infrastructure.MiddleWares;
using Persistance.Seeder;

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
                 .AddInfrastructureDependencies(builder.Configuration)
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
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    await SeedData.SeedAsync(dbContext, userManager, roleManager);
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

//app.UseSerilogRequestLogging();
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