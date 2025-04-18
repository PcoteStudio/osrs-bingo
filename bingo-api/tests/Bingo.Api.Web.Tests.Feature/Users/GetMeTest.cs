using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Users;

public partial class UsersFeatureTest
{
    [Test]
    public async Task GetMeAsync_ShouldAutomaticallyGiveAccessToGetMe()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithSecrets);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var getMeResponse = await _client.GetAsync(new Uri(_baseUrl, "/api/users/me"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, getMeResponse);

        // Assert response content
        var responseContent = await getMeResponse.Content.ReadAsStringAsync();
        var returnedUser = JsonSerializer.Deserialize<UserResponse>(responseContent, JsonSerializerOptions.Web);
        returnedUser.Should().NotBeNull();
        returnedUser.Username.Should().Be(userWithSecrets.User.Username);
        returnedUser.Email.Should().Be(userWithSecrets.User.Email);
    }

    [Test]
    public async Task GetMeAsync_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup.AddUser();

        // Act
        var getMeResponse = await _client.GetAsync(new Uri(_baseUrl, "/api/users/me"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, getMeResponse);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(getMeResponse);
    }
}