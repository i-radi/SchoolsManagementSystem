using Microsoft.AspNetCore.Http;
using Persistance.Context;
using System.Security.Claims;

namespace Infrastructure.MiddleWares;

public class SchoolAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public SchoolAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ApplicationDBContext dbContext)
    {
        var userId = context.User.FindFirst("id")?.Value;
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (!string.IsNullOrEmpty(userId) && userRole == "Admin")
        {
            var schoolId = context.Request.Headers["schoolId"].ToString();
            if (string.IsNullOrEmpty(schoolId)
                || !(await IsAdminAuthorizedForSchool(userId, schoolId, dbContext)))
            {
                context.Response.StatusCode = 403;
                return;
                //throw new UnauthorizedAccessException();
            }
        }
        await _next(context);
    }

    private async Task<bool> IsAdminAuthorizedForSchool(string? adminId, string? schoolId, ApplicationDBContext dbContext)
    {
        //var adminSchoolId = (await dbContext.Users.FindAsync(Int32.Parse(adminId!)))?.SchoolId;
        //if (adminSchoolId is not null
        //    && !string.IsNullOrEmpty(schoolId)
        //    && adminSchoolId == (Int32.Parse(schoolId!)))
        //{
        //    return true;
        //}
        return true;
    }
}
