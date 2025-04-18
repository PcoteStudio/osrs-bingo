using System.Net;
using System.Net.Http.Headers;
using Bingo.Api.TestUtils;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Authentication;

public partial class AuthenticationFeatureTest
{
    [Test]
    public async Task LogoutAsync_ShouldPreventAccessToGetMe()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithSecrets);
        var stringContent = new StringContent("", new MediaTypeHeaderValue("application/json"));

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var logoutResponse = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/logout"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, logoutResponse);

        // Assert response content
        var responseContent = await logoutResponse.Content.ReadAsStringAsync();
        responseContent.Should().BeEmpty();

        // Act
        var meResponse = await _client.GetAsync(new Uri(_baseUrl, "/api/users/me"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, meResponse);
    }
}