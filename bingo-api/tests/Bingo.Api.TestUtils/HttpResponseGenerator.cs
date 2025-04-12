using System.Net;

namespace Bingo.Api.TestUtils;

public static class HttpResponseGenerator
{
    public static string GetExpectedJsonResponse(HttpStatusCode statusCode)
    {
        return BuildJsonResponse((int)statusCode, "The requested resource was not found");
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