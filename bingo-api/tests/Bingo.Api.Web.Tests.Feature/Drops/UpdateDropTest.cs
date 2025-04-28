using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Drops;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Drops;

public partial class DropFeatureTest
{
    [Test]
    public async Task UpdateDropAsync_ShouldReturnTheUpdatedDrop()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.update")
            .AddNpc()
            .AddItem()
            .AddDrop(out var drop)
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropUpdateArguments(npc.Id, item.Id);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{drop.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedDrop = JsonSerializer.Deserialize<DropResponse>(responseContent, JsonSerializerOptions.Web);
        returnedDrop.Should().NotBeNull();
        var savedDrop = _dbContext.Drops.FirstOrDefault(d => d.Id == returnedDrop.Id);
        savedDrop.Should().NotBeNull();
        returnedDrop.Id.Should().Be(savedDrop.Id);
        returnedDrop.NpcId.Should().Be(savedDrop.NpcId).And.Be(args.NpcId).And.Be(args.NpcId);
        returnedDrop.ItemId.Should().Be(savedDrop.ItemId).And.Be(args.ItemId).And.Be(args.ItemId);
        returnedDrop.Ehc.Should().Be(savedDrop.Ehc).And.BePositive();
    }

    [Test]
    public async Task UpdateDropAsync_ShouldReturnNotFoundIfDropDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.update");
        const int dropId = 1_000_000;
        var args = TestDataGenerator.GenerateDropUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{dropId}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateDropAsync_ShouldReturnNotFoundIfNpcDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.update")
            .AddNpc()
            .AddItem()
            .AddDrop(out var drop)
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropUpdateArguments(npc.Id * 10_000, item.Id);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{drop.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateDropAsync_ShouldReturnNotFoundIfItemDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.update")
            .AddNpc()
            .AddItem()
            .AddDrop(out var drop)
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropUpdateArguments(npc.Id, item.Id * 10_000);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{drop.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateDropAsync_ShouldReturnConflictIfDropAlreadyExists()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.update")
            .AddNpc(out var item)
            .AddItem(out var npc)
            .AddDrop()
            .AddNpc()
            .AddItem()
            .AddDrop(out var drop);
        var args = TestDataGenerator.GenerateDropUpdateArguments(npc.Id, item.Id);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{drop.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Conflict, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateDropAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddNpc()
            .AddItem()
            .AddDrop(out var drop)
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropUpdateArguments(npc.Id, item.Id);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{drop.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateDropAsync_ShouldReturnIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddNpc()
            .AddItem()
            .AddDrop(out var drop)
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropUpdateArguments(npc.Id, item.Id);

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/drops/{drop.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}