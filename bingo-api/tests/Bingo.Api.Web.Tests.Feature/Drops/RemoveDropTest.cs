using System.Net;
using Bingo.Api.TestUtils;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Drops;

public partial class DropFeatureTest
{
    [Test]
    public async Task RemoveDropAsync_ShouldReturnNoContent()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.delete")
            .AddItem()
            .AddNpc()
            .AddDrop(out var drop);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/drops/{drop.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NoContent, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveDropAsync_ShouldReturnNotFoundIfDropDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.delete");
        const int dropId = 1_000_000;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(new Uri(_baseUrl, $"/api/drops/{dropId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveDropAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddItem()
            .AddNpc()
            .AddDrop(out var drop);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/drops/{drop.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveDropAsync_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("drop.delete")
            .AddItem()
            .AddNpc()
            .AddDrop(out var drop);

        // Act
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/drops/{drop.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}