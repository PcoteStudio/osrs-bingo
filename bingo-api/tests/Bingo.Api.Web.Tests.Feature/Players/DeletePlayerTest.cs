using System.Net;
using System.Text.Json;
using Bingo.Api.TestUtils;
using Bingo.Api.Web.Players;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Players;

public partial class PlayerFeatureTest
{
    [Test]
    public async Task DeletePlayerAsync_ShouldReturnSuccess()
    {
        // Arrange
        _testDataSetup
            .AddUser(out var userWithSecrets)
            .AddPermissions("team.update")
            .AddEvent()
            .AddTeam(out var originalTeam)
            .AddPlayer(out var originalPlayer)
            .AddPlayer(out var otherPlayer);

        // Act
        await AuthenticationHelper.LoginWithClient(_client, _baseUrl, userWithSecrets);
        var response = await _client.DeleteAsync(new Uri(_baseUrl, $"/api/players/{originalPlayer.Id}"));

        // Assert response status
        await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response);

        // Assert response content
        var responseContent = await response.Content.ReadAsStringAsync();
        var returnedPlayer = JsonSerializer.Deserialize<PlayerResponse>(responseContent, JsonSerializerOptions.Web);
        var deletedPlayer = await _dbContext.Players.FindAsync(originalPlayer.Id);
        var updatedTeam =
            await _dbContext.Teams.Include(t => t.Players).FirstOrDefaultAsync(t => t.Id == originalTeam.Id);
        returnedPlayer.Should().NotBeNull();
        deletedPlayer.Should().BeNull();
        updatedTeam.Should().NotBeNull();

        returnedPlayer.Id.Should().Be(originalPlayer.Id);
        returnedPlayer.Name.Should().Be(originalPlayer.Name);
        updatedTeam.Players.Count.Should().Be(1);
        updatedTeam.PlayerIds.Should().BeEquivalentTo([otherPlayer.Id]);
    }
}