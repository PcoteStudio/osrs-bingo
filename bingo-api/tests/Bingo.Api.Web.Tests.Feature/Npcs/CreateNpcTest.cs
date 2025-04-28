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
    public async Task CreateNpc_ShouldReturnTheCreatedNpc()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("npc.create");
        var args = TestDataGenerator.GenerateNpcCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/npcs"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

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
    public async Task CreateNpc_ShouldReturnForbiddenIfNotTheNpcAdmin()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddUser();
        var args = TestDataGenerator.GenerateNpcCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/npcs"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateNpc_ShouldReturnForbiddenIfMissingPermissions()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets);
        var args = TestDataGenerator.GenerateNpcCreateArguments();

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/npcs"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Forbidden, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }

    [Test]
    public async Task CreateNpc_ShouldReturnUnauthorizedIfNotLoggedIn()
    {
        // Arrange
        _testDataSetup
            .AddUser();
        var args = TestDataGenerator.GenerateNpcCreateArguments();

        // Act
        var response = await _client.PostAsync(new Uri(_baseUrl, "/api/npcs"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.Unauthorized, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}