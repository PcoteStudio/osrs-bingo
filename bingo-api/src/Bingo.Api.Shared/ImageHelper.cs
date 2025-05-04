using Microsoft.AspNetCore.Http;

namespace Bingo.Api.Shared;

public static class ImageHelper
{
    public static string IFormFileToBase64(IFormFile file)
    {
        using var fileStream = file.OpenReadStream();
        var bytes = new byte[file.Length];
        fileStream.ReadExactly(bytes, 0, (int)file.Length);
        return Convert.ToBase64String(bytes);
    }
}