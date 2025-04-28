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
    public async Task CreateDrop_ShouldReturnTheCreatedDrop()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.create")
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropCreateArguments();
        args.NpcId = npc.Id;
        args.ItemId = item.Id;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/drops"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedDrop = JsonSerializer.Deserialize<DropResponse>(responseContent, JsonSerializerOptions.Web);
        returnedDrop.Should().NotBeNull();
        var savedDrop = _dbContext.Drops.FirstOrDefault(d => d.Id == returnedDrop.Id);
        savedDrop.Should().NotBeNull();
        returnedDrop.Id.Should().Be(savedDrop.Id);
        returnedDrop.NpcId.Should().Be(savedDrop.NpcId).And.Be(args.NpcId);
        returnedDrop.ItemId.Should().Be(savedDrop.ItemId).And.Be(args.ItemId);
        returnedDrop.Ehc.Should().Be(savedDrop.Ehc).And.BePositive();
    }

    [Test]
    public async Task CreateDrop_ShouldReturnNotFoundIfNpcDoesntExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.create")
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropCreateArguments();
        args.NpcId = npc.Id * 10_000;
        args.ItemId = item.Id;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/drops"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateDrop_ShouldReturnNotFoundIfItemDoesntExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.create")
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropCreateArguments();
        args.NpcId = npc.Id;
        args.ItemId = item.Id * 10_000;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/drops"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateDrop_ShouldReturnBadRequestIfDropAlreadyExists()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("drop.create")
            .AddNpc(out var npc)
            .AddItem(out var item)
            .AddDrop();
        var args = TestDataGenerator.GenerateDropCreateArguments();
        args.NpcId = npc.Id;
        args.ItemId = item.Id;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/drops"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.BadRequest, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateDrop_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropCreateArguments();
        args.NpcId = npc.Id;
        args.ItemId = item.Id;

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/drops"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateDrop_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("drop.create")
            .AddNpc(out var npc)
            .AddItem(out var item);
        var args = TestDataGenerator.GenerateDropCreateArguments();
        args.NpcId = npc.Id;
        args.ItemId = item.Id;

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/drops"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}