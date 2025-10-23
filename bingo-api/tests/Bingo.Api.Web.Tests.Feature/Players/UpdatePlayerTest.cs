using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Players;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Players;

public partial class PlayerFeatureTest
{
    [Test]
    public async Task UpdatePlayerAsync_ShouldReturnTheUpdatedPlayer()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayer(out var originalPlayer)
            .AddPlayer();
        var args = TestDataGenerator.GeneratePlayerUpdateArguments();
        args.TeamIds = [originalTeam.Id];

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.PutAsync(new Uri(_baseUrl, $"/api/players/{originalPlayer.Id}"),
            HttpHelper.BuildJsonStringContent(args));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedPlayer = JsonSerializer.Deserialize<PlayerResponse>(responseContent, JsonSerializerOptions.Web);
        var updatedPlayer = _dbContext.Players.Include(p => p.Teams).FirstOrDefault(p => p.Id == originalPlayer.Id);
        returnedPlayer.Should().NotBeNull();
        updatedPlayer.Should().NotBeNull();

        returnedPlayer.Name.Should().Be(updatedPlayer.Name).And.Be(args.Name);
        returnedPlayer.Id.Should().Be(updatedPlayer.Id).And.Be(originalPlayer.Id);
        returnedPlayer.TeamIds.Should().BeEquivalentTo(updatedPlayer.TeamIds).And
            .BeEquivalentTo(args.TeamIds);
    }
}