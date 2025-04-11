using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Data;
using Bingo.Api.TestUtils.TestDataGenerators;
using Moq;

namespace Bingo.Api.Core.Tests.Features.Teams;

[TestFixture]
[TestOf(typeof(TeamService))]
public class TeamServiceUnitTest
{
    [SetUp]
    public void BeforeEach()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>(MockBehavior.Strict);
        _teamFactoryMock = new Mock<ITeamFactory>(MockBehavior.Strict);
        _teamUtilMock = new Mock<ITeamUtil>(MockBehavior.Strict);
        _playerServiceMock = new Mock<IPlayerService>(MockBehavior.Strict);
        _dbContextMock = new Mock<ApplicationDbContext>(MockBehavior.Loose);
        _service = new TeamService(
            _teamFactoryMock.Object,
            _teamRepositoryMock.Object,
            _teamUtilMock.Object,
            _playerServiceMock.Object,
            _dbContextMock.Object
        );
    }

    private TeamService _service;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITeamFactory> _teamFactoryMock;
    private Mock<ITeamUtil> _teamUtilMock;
    private Mock<IPlayerService> _playerServiceMock;
    private Mock<ApplicationDbContext> _dbContextMock;

    [Test]
    public async Task CreateTeam_ShouldCreateATeamWithTheSpecifiedArgumentsAndReturnIt()
    {
        // Arrange
        var eventId = Random.Shared.Next();
        var teamArgs = TestDataGenerator.GenerateTeamCreateArguments();
        var team = TestDataGenerator.GenerateTeamEntity();

        _teamFactoryMock.Setup(x => x.Create(eventId, teamArgs))
            .Returns(team).Verifiable(Times.Once);
        _teamRepositoryMock.Setup(x => x.Add(team))
            .Verifiable(Times.Once);
        _dbContextMock.Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1).Verifiable(Times.Once);

        // Act
        var actualTeam = await _service.CreateTeamAsync(eventId, teamArgs);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamFactoryMock, _teamRepositoryMock, _dbContextMock);
    }

    [Test]
    public async Task GetEventTeamsAsync_ShouldReturnAllTeams()
    {
        // Arrange
        var eventId = Random.Shared.Next();
        var teams = TestDataGenerator.GenerateTeamEntities(3);

        _teamRepositoryMock.Setup(x => x.GetAllByEventIdAsync(eventId))
            .ReturnsAsync(teams).Verifiable(Times.Once);

        // Act
        var actualTeams = await _service.GetEventTeamsAsync(eventId);

        // Assert
        Assert.That(actualTeams, Is.EquivalentTo(teams));
        Mock.VerifyAll(_teamRepositoryMock);
    }

    [Test]
    public async Task GetRequiredTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();

        _teamRepositoryMock.Setup(x => x.GetCompleteByIdAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);

        // Act
        var actualTeam = await _service.GetRequiredTeamAsync(team.Id);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamRepositoryMock);
    }
}