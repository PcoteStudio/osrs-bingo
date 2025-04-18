using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Users;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Authentication;

public partial class AuthenticationFeatureTest
{
    [Test]
    public async Task SignupAsync_ShouldReturnTheCreatedUser()
    {
        // Arrange
        var args = TestDataGenerator.GenerateAuthSignupArguments();
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var signupResponse = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/signup"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, signupResponse);

        // Assert response content
        var responseContent = await signupResponse.Content.ReadAsStringAsync();
        var returnedUser = JsonSerializer.Deserialize<UserResponse>(responseContent, JsonSerializerOptions.Web);
        returnedUser.Should().NotBeNull();
        returnedUser.Username.Should().Be(args.Username);
        returnedUser.Email.Should().Be(args.Email);
    }

    [Test]
    public async Task SignupAsync_ShouldReturnABadRequestIfTheUsernameIsTaken()
    {
        // Arrange
        _testDataSetup.AddUser(out var userWithSecrets);
        var args = TestDataGenerator.GenerateAuthSignupArguments(args =>
        {
            args.Username = userWithSecrets.User.Username;
        });
        var postContent = JsonSerializer.Serialize(args);
        var stringContent = new StringContent(postContent, new MediaTypeHeaderValue("application/json"));

        // Act
        var signupResponse = await _client.PostAsync(new Uri(_baseUrl, "/api/auth/signup"), stringContent);

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.BadRequest, signupResponse);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(signupResponse);
    }
}