using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Npcs;
using FluentAssertions;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Npcs;

public partial class NpcFeatureTest
{
    [Test]
    public async Task GetNpcAsync_ShouldReturnTheSpecifiedNpc()
    {
        // Arrange
        _testDataSetup.AddNpc(out var npcEntity);

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/npcs/{npcEntity.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedNpc = JsonSerializer.Deserialize<NpcResponse>(responseContent, JsonSerializerOptions.Web);
        returnedNpc.Should().NotBeNull();
        returnedNpc.Id.Should().Be(npcEntity.Id);
        returnedNpc.Name.Should().Be(npcEntity.Name);
        returnedNpc.Image.Should().Be(npcEntity.Image);
        returnedNpc.KillsPerHour.Should().Be(npcEntity.KillsPerHour);
    }

    [Test]
    public async Task GetNpcAsync_ShouldReturnNotFoundIfNpcDoesNotExist()
    {
        // Arrange
        const int npcId = 1_000_000;

        // Act
        var response = await _client.GetAsync(new Uri(_baseUrl, $"/api/npcs/{npcId}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.NotFound, response);

        // Assert response content
        await Expect.ResponseContentToMatchStatusCode(response);
    }
}