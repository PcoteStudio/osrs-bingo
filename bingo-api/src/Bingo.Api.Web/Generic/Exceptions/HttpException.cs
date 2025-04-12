namespace Bingo.Api.Web.Generic.Exceptions;

public class HttpException : Exception
{
    public HttpException(int statusCode, string message, Exception innerException)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public HttpException(int statusCode, Exception innerException)
        : base(GetMessage(statusCode), innerException)
    {
        StatusCode = statusCode;
    }

    public HttpException(int statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }

    public int StatusCode { get; }

    private static string GetMessage(int statusCode)
    {
        switch (statusCode)
        {
            case StatusCodes.Status401Unauthorized:
                return "Unauthorized";
            case StatusCodes.Status403Forbidden:
                return "You are not allowed to access this resource";
            case StatusCodes.Status404NotFound:
                return "The requested resource was not found";
            default:
                return "An error occurred";
        }
    }
}