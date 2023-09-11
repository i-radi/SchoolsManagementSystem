using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.MiddleWares;

public class RequestResponseLoggingMiddleware : IMiddleware
{
    private readonly ILogger<RequestResponseLoggingMiddleware> logger;

    public RequestResponseLoggingMiddleware(ILogger<RequestResponseLoggingMiddleware> logger)
    {
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        bool IsApiRequest = httpContext.Request.Path.ToString().Contains("/api/");
        if (IsApiRequest)
        {

            var request = await ReadBodyFromRequest(httpContext.Request);

            logger.LogInformation("Request: {request}", request);

            var originalResponseBody = httpContext.Response.Body;
            using var newResponseBody = new MemoryStream();
            httpContext.Response.Body = newResponseBody;

            await next(httpContext);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

            logger.LogInformation("Response: {response}", responseBodyText);

            newResponseBody.Seek(0, SeekOrigin.Begin);
            await newResponseBody.CopyToAsync(originalResponseBody);
        }
        else
        {
            await next(httpContext);
        }
    }

    private static async Task<string> ReadBodyFromRequest(HttpRequest request)
    {
        request.EnableBuffering();

        using var streamReader = new StreamReader(request.Body, leaveOpen: true);
        var requestBody = await streamReader.ReadToEndAsync();

        request.Body.Position = 0;
        return requestBody;
    }
}
