using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;

namespace Bingo.Api.Web.Tests;

public static class AuthenticationHelper
{
    public static async Task LoginWithClient(HttpClient client, Uri baseUrl, string username, string password)
    {
        var loginArgs = new AuthLoginArguments
        {
            Username = username,
            Password = password
        };
        var postContent = JsonSerializer.Serialize(loginArgs);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));
        var loginResponse = await client.PostAsync(new Uri(baseUrl, "/api/auth/login"), stringContent);
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, loginResponse);
    }

    public static Task LoginWithClient(HttpClient client, Uri baseUrl, UserWithSecrets user)
    {
        return LoginWithClient(client, baseUrl, user.User.Username, user.Password);
    }
}