using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMS.Persistance.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region Connection To SQL Server

builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection"));
});

#endregion

#region Dependency injections

builder.Services.AddPersistanceDependencies()
                 .AddInfrastructureDependencies()
                 .AddCoreDependencies()
                 .AddPersistanceServiceRegisteration(builder.Configuration)
                 .AddInfrastructureServiceRegisteration(builder.Configuration);

#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
