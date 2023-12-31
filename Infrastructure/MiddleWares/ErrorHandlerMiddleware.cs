﻿using Infrastructure.Bases;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Helpers;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Infrastructure.MiddleWares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly EmailSettings _emailSettings;
    private readonly IEmailService _emailSender;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, EmailSettings emailSettings, IEmailService emailSender)
    {
        _next = next;
        _logger = logger;
        _emailSettings = emailSettings;
        _emailSender = emailSender;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            _logger.LogError(error.ToString());

            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new Result<string>() { Succeeded = false, Message = error?.Message! };

            if (_emailSettings.SendEmails)
            {
                var userName = context.User.FindFirst(ClaimTypes.Name)?.Value;
                var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
                var message = $"{error!.Message}</br>" +
                    $" Time : {DateTime.Now.ToShortTimeString()}</br>" +
                    $" User Email : {userName}</br> User Role : {userRole}</br>" +
                    $" Response Model : {JsonSerializer.Serialize(responseModel)}";
                _emailSender.SendEmail(message);
            }

            switch (error)
            {
                case UnauthorizedAccessException:
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.Unauthorized;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ValidationException:
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;

                case KeyNotFoundException:
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case DbUpdateException e:
                    responseModel.Message = e.Message;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case Exception e:
                    if (e.GetType().ToString() == "ApiException")
                    {
                        responseModel.Message += e.Message;
                        responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    responseModel.Message = e.Message;
                    responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;

                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    responseModel.Message = error!.Message!;
                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);
        }
    }
}
