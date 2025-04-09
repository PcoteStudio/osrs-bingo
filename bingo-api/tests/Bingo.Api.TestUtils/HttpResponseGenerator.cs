using System.Net;

namespace Bingo.Api.TestUtils;

public static class HttpResponseGenerator
{
    public static string GetExpectedJsonResponse(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.NotFound:
                return BuildJsonResponse((int)HttpStatusCode.NotFound, "Not Found",
                    "https://tools.ietf.org/html/rfc9110#section-15.5.5");
            default:
                throw new NotImplementedException("No implemented response for status code: " + statusCode);
        }
    }

    private static string BuildJsonResponse(int code, string title, string type)
    {
        return /* language=json */
            $$"""
              {
                "status": {{code}},
                "title": "{{title}}",
                "traceId": { "__match": {  "type": "string"  } },
                "type": "{{type}}"
              }
              """;
    }
}