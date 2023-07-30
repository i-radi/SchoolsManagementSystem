namespace SMS.Infrastructure.Bases;

public static class ResponseHandler
{
    public static Response<T> Deleted<T>(string Message = null)
    {
        return new Response<T>()
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Succeeded = true,
            Message = Message == null ? "Deleted Successfully" : Message
        };
    }
    public static Response<T> Success<T>(T entity, object Meta = null)
    {
        return new Response<T>()
        {
            Data = entity,
            StatusCode = System.Net.HttpStatusCode.OK,
            Succeeded = true,
            Message = "operation is done",
            Meta = Meta
        };
    }
    public static Response<T> Unauthorized<T>(string Message = null)
    {
        return new Response<T>()
        {
            StatusCode = System.Net.HttpStatusCode.Unauthorized,
            Succeeded = true,
            Message = Message == null ? "You are not Authorized To Access This Resources" : Message
        };
    }
    public static Response<T> BadRequest<T>(string Message = null)
    {
        return new Response<T>()
        {
            StatusCode = System.Net.HttpStatusCode.BadRequest,
            Succeeded = false,
            Message = Message == null ? "Request Can't be Understand" : Message
        };
    }
    public static Response<T> UnprocessableEntity<T>(string Message = null)
    {
        return new Response<T>()
        {
            StatusCode = System.Net.HttpStatusCode.UnprocessableEntity,
            Succeeded = false,
            Message = Message == null ? "There are Validation Errors Or Syntax Errors" : Message
        };
    }
    public static Response<T> NotFound<T>(string message = null)
    {
        return new Response<T>()
        {
            StatusCode = System.Net.HttpStatusCode.NotFound,
            Succeeded = false,
            Message = message == null ? "Not Found" : message
        };
    }
    public static Response<T> Created<T>(T entity, object Meta = null)
    {
        return new Response<T>()
        {
            Data = entity,
            StatusCode = System.Net.HttpStatusCode.Created,
            Succeeded = true,
            Message = "Created Successfully",
            Meta = Meta
        };
    }
}