using System.Net;
using Bingo.Api.TestUtils;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Items;

public partial class ItemFeatureTest
{
    [Test]
    public async Task RemoveItemAsync_ShouldReturnNoContent()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("item.delete")
            .AddItem(out var item);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/items/{item.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NoContent, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveItemAsync_ShouldReturnNotFoundIfItemDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("item.delete");
        const int itemId = 1_000_000;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(new Uri(_baseUrl, $"/api/items/{itemId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveItemAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddItem(out var item);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/items/{item.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveItemAsync_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("item.delete")
            .AddItem(out var item);

        // Act
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/items/{item.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}