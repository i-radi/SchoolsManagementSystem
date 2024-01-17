using Infrastructure.MiddleWares;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

#region Services

#region Connection To SQL Server
builder.Services.AddMiniProfiler(options => options.RouteBasePath = "/profiler").AddEntityFramework();
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDbContextConnection")!);
}, ServiceLifetime.Transient);

#endregion

#region Dependency injections
#pragma warning disable CS0612 // Type or member is obsolete
builder.Services.AddSerilogRegisteration(builder.Configuration);
#pragma warning restore CS0612 // Type or member is obsolete

builder.Services.AddPersistanceDependencies()
                 .AddInfrastructureDependencies(builder.Configuration)
                 .AddCoreDependencies()
                 .AddPersistanceServiceRegisteration(builder.Configuration)
                 .AddInfrastructureServiceRegisteration(builder.Configuration);

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

#endregion

#region API

builder.Services.AddEndpointsApiExplorer();

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

#region Intiate database

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    dbContext.Database.Migrate();
}

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseMiniProfiler();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity");
        options.SwaggerEndpoint("/swagger/Users/swagger.json", "Users");
        options.SwaggerEndpoint("/swagger/Organizations/swagger.json", "Organizations");
        options.SwaggerEndpoint("/swagger/Schools/swagger.json", "Schools");
        options.SwaggerEndpoint("/swagger/Seasons/swagger.json", "Seasons");
        options.SwaggerEndpoint("/swagger/Grades/swagger.json", "Grades");
        options.SwaggerEndpoint("/swagger/Classes/swagger.json", "Classes");
        options.SwaggerEndpoint("/swagger/Courses/swagger.json", "Courses");
        options.SwaggerEndpoint("/swagger/Activities/swagger.json", "Activities");
        options.SwaggerEndpoint("/swagger/Records/swagger.json", "Records");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
        options.DefaultModelsExpandDepth(-1);
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(CORS);

app.UseAuthentication();
app.UseAuthorization();


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