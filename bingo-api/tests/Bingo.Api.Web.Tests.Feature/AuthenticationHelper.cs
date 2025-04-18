using System.Net;
using Bingo.Api.Core.Features.Authentication.Arguments;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;

namespace Bingo.Api.Web.Tests.Feature;

public static class AuthenticationHelper
{
    public static async Task LoginWithClient(HttpClient client, Uri baseUrl, string username, string password)
    {
        var args = new AuthLoginArguments
        {
            Username = username,
            Password = password
        };
        var loginResponse =
            await client.PostAsync(new Uri(baseUrl, "/api/auth/login"), HttpHelper.BuildJsonStringContent(args));
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, loginResponse);
    }

    public static Task LoginWithClient(HttpClient client, Uri baseUrl, UserWithSecrets user)
    {
        return LoginWithClient(client, baseUrl, user.User.Username, user.Password);
    }
}