using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Bingo.Api.Shared;

namespace Bingo.Api.TestUtils;

public static class HttpHelper
{
    public static async Task<ByteArrayContent> BuildFileContent(string fileDirectory, string fileName)
    {
        var filePath = FileSystemHelper.FindFileOfDirectory(fileDirectory, fileName);
        var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
        var mediaType = Path.GetExtension(filePath) switch
        {
            ".svg" => "image/svg+xml",
            ".png" => "image/png",
            _ => "application/octet-stream"
        };
        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(mediaType);
        return fileContent;
    }

    public static StringContent BuildJsonStringContent<TValue>(TValue value)
    {
        var postContent = JsonSerializer.Serialize(value);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        return stringContent;
    }

    public static string GetExpectedJsonResponse(HttpStatusCode statusCode)
    {
        switch (statusCode)
        {
            case HttpStatusCode.NoContent:
                return "";
            case HttpStatusCode.BadRequest:
                return BuildJsonResponse((int)statusCode, "Bad request");
            case HttpStatusCode.Unauthorized:
                return BuildJsonResponse((int)statusCode, "Unauthorized");
            case HttpStatusCode.Forbidden:
                return BuildJsonResponse((int)statusCode, "You are not allowed to access this resource");
            case HttpStatusCode.NotFound:
                return BuildJsonResponse((int)statusCode, "The requested resource was not found");
            case HttpStatusCode.Conflict:
                return BuildJsonResponse((int)statusCode, "There was a conflict with the resource");
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