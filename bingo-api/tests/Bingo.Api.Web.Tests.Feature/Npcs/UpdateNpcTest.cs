using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Npcs;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Npcs;

public partial class NpcFeatureTest
{
    [Test]
    public async Task UpdateNpcAsync_ShouldReturnTheUpdatedNpc()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("npc.update")
            .AddNpc(out var originalNpc);
        var args = TestDataGenerator.GenerateNpcUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/npcs/{originalNpc.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedNpc = JsonSerializer.Deserialize<NpcResponse>(responseContent, JsonSerializerOptions.Web);
        returnedNpc.Should().NotBeNull();
        var savedNpc = _dbContext.Npcs.Include(i => i.Drops).FirstOrDefault(t => t.Id == returnedNpc.Id);
        savedNpc.Should().NotBeNull();
        returnedNpc.Id.Should().Be(savedNpc.Id);
        returnedNpc.Name.Should().Be(savedNpc.Name).And.Be(args.Name);
        returnedNpc.Image.Should().Be(savedNpc.Image).And.Be(args.Image);
        returnedNpc.KillsPerHour.Should().Be(savedNpc.KillsPerHour).And.Be(args.KillsPerHour);
        returnedNpc.Drops.Count.Should().Be(savedNpc.Drops.Count).And.Be(0);
    }

    [Test]
    public async Task UpdateNpcAsync_ShouldReturnNotFoundIfNpcDoesNotExist()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("npc.update");
        const int npcId = 1_000_000;
        var args = TestDataGenerator.GenerateNpcUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/npcs/{npcId}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateNpcAsync_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddNpc(out var originalNpc);
        var args = TestDataGenerator.GenerateNpcUpdateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/npcs/{originalNpc.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task UpdateNpcAsync_ShouldReturnIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser()
            .AddPermissions("npc.update")
            .AddNpc(out var originalNpc);
        var args = TestDataGenerator.GenerateNpcUpdateArguments();

        // Act
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/npcs/{originalNpc.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}