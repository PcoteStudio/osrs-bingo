using Bingo.Api.Core.Features.Teams;
using Bingo.Api.TestUtils.TestDataGenerators;
using FluentAssertions;

namespace Bingo.Api.Core.Tests.Features.Teams;

[TestFixture]
[TestOf(typeof(TeamFactory))]
public class TeamFactoryUnitTest
{
    [OneTimeSetUp]
    public void BeforeAll()
    {
        _teamFactory = new TeamFactory();
    }

    private ITeamFactory _teamFactory;

    [Test]
    public void Create_ShouldReturnAProperlyMappedTeamEntity()
    {
        // Arrange
        var eventId = Random.Shared.Next();
        var args = TestDataGenerator.GenerateTeamCreateArguments();

        // Act
        var createdTeam = _teamFactory.Create(eventId, args);

        // Assert
        createdTeam.Should().NotBeNull();
        createdTeam.EventId.Should().Be(eventId);
        createdTeam.Name.Should().Be(args.Name);
    }

    [Test]
    public void Create_ShouldNotSetTheIdOfTheTeamEntity()
    {
        // Arrange
        var eventId = Random.Shared.Next();
        var args = TestDataGenerator.GenerateTeamCreateArguments();

        // Act
        var createdTeam = _teamFactory.Create(eventId, args);

        // Assert
        createdTeam.Should().NotBeNull();
        createdTeam.Id.Should().Be(0);
    }
}