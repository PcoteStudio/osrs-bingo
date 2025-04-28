using System.Net;
using Bingo.Api.TestUtils;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Npcs;

public partial class NpcFeatureTest
{
    [Test]
    public async Task RemoveNpcAsync_ShouldReturnNoContent()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("npc.delete")
            .AddNpc(out var npc);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/npcs/{npc.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NoContent, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveNpcAsync_ShouldReturnNotFoundIfNpcDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("npc.delete");
        const int npcId = 1_000_000;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(new Uri(_baseUrl, $"/api/npcs/{npcId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveNpcAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddNpc(out var npc);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/npcs/{npc.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task RemoveNpcAsync_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("npc.delete")
            .AddNpc(out var npc);

        // Act
        var response = await _client.DeleteAsync(
            new Uri(_baseUrl, $"/api/npcs/{npc.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}