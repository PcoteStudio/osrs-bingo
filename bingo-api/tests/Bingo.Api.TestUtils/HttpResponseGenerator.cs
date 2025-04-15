using System.Net;

namespace Bingo.Api.TestUtils;

public static class HttpResponseGenerator
{
    public static string GetExpectedJsonResponse(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.BadRequest:
                return BuildJsonResponse((int)statusCode, "Bad request");
            case HttpStatusCode.Unauthorized:
                return BuildJsonResponse((int)statusCode, "Unauthorized");
            case HttpStatusCode.Forbidden:
                return BuildJsonResponse((int)statusCode, "You are not allowed to access this resource");
            case HttpStatusCode.NotFound:
                return BuildJsonResponse((int)statusCode, "The requested resource was not found");
            default:
                throw new Exception("Not implemented");
        }
    }

    private static string BuildJsonResponse(int code, string message)
    {
        return /* language=json */
            $$"""
              {
                "status": {{code}},
                "message": "{{message}}"
              }
              """;
    }
}