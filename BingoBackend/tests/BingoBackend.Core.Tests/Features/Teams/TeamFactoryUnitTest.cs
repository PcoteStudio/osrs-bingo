using BingoBackend.Core.Features.Teams;
using BingoBackend.TestUtils.TestDataSetup;

namespace BingoBackend.Core.Tests.Features.Teams;

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
        var args = TestDataSetup.GenerateTeamCreateArguments();

        // Act
        var createdTeam = _teamFactory.Create(args);

        // Assert
        Assert.That(createdTeam, Is.Not.Null);
        Assert.That(createdTeam.Name, Is.EqualTo(args.Name));
    }

    [Test]
    public void Create_ShouldNotSetTheIdOfTheTeamEntity()
    {
        // Arrange
        var args = TestDataSetup.GenerateTeamCreateArguments();

        // Act
        var createdTeam = _teamFactory.Create(args);

        // Assert
        Assert.That(createdTeam, Is.Not.Null);
        Assert.That(createdTeam.Id, Is.Default);
    }
}