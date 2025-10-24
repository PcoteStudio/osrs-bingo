using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Teams;
using TeamMapper = Bingo.Api.Web.Players.TeamMapper;

namespace Bingo.Api.Web.Tests.Unit.Teams;

[TestFixture]
[TestOf(typeof(TeamMapper))]
public class TeamMapperUnitTest
{
    [Test]
    public void ToShortResponse_ShouldProperlyMapToShortResponse()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();

        // Act
        var teamResponse = team.ToShortResponse();

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(teamResponse.Id, Is.EqualTo(team.Id));
            Assert.That(teamResponse.Name, Is.EqualTo(team.Name));
        });
    }
}