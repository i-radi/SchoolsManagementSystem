namespace Infrastructure.Bases;

public static class ResultHandler
{
    public static Result<T> Deleted<T>(string? Message = null)
    {
        return new Result<T>()
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Succeeded = true,
            Message = Message ?? "Deleted Successfully"
        };
    }
    public static Result<T> Success<T>(T entity)
    {
        return new Result<T>()
        {
            Data = entity,
            StatusCode = System.Net.HttpStatusCode.OK,
            Succeeded = true,
            Message = "operation is done"
        };
    }
    public static Result<T> Unauthorized<T>(string? Message = null)
    {
        return new Result<T>()
        {
            StatusCode = System.Net.HttpStatusCode.Unauthorized,
            Succeeded = true,
            Message = Message ?? "You are not Authorized To Access This Resources"
        };
    }
    public static Result<T> BadRequest<T>(string? Message = null)
    {
        return new Result<T>()
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = Message ?? "Request Can't be Understand"
        };
    }
    public static Result<T> UnprocessableEntity<T>(string? Message = null)
    {
        return new Result<T>()
        {
            StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
            Succeeded = false,
            Message = Message ?? "There are Validation Errors Or Syntax Errors"
        };
    }
    public static Result<T> NotFound<T>(string? message = null)
    {
        return new Result<T>()
        {
            StatusCode = System.Net.HttpStatusCode.NotFound,
            Succeeded = false,
            Message = message ?? "Not Found"
        };
    }
    public static Result<T> Created<T>(T entity)
    {
        return new Result<T>()
        {
            Data = entity,
            StatusCode = System.Net.HttpStatusCode.Created,
            Succeeded = true,
            Message = "Created Successfully"
        };
    }
}