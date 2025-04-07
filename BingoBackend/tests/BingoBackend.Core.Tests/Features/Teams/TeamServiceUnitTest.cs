using BingoBackend.Core.Features.Players;
using BingoBackend.Core.Features.Teams;
using BingoBackend.Core.Features.Teams.Arguments;
using BingoBackend.Data;
using BingoBackend.Data.Entities;
using Moq;

namespace BingoBackend.Core.Tests.Features.Teams;

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
        var teamArguments = new TeamCreateArguments();
        var team = new TeamEntity();

        _teamFactoryMock.Setup(x => x.Create(teamArguments))
            .Returns(team).Verifiable(Times.Once);
        _teamRepositoryMock.Setup(x => x.Add(team))
            .Verifiable(Times.Once);
        _dbContextMock.Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1).Verifiable(Times.Once);

        // Act
        var actualTeam = await _service.CreateTeamAsync(teamArguments);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamFactoryMock, _teamRepositoryMock, _dbContextMock);
    }

    [Test]
    public async Task GetTeams_ShouldReturnAllTeams()
    {
        // Arrange
        const int entitiesCount = 3;
        var teams = Enumerable.Range(0, entitiesCount).Select(_ => new TeamEntity()).ToList();

        _teamRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(teams).Verifiable(Times.Once);

        // Act
        var actualTeams = await _service.GetTeamsAsync();

        // Assert
        Assert.That(actualTeams, Is.EquivalentTo(teams));
        Mock.VerifyAll(_teamRepositoryMock);
    }

    [Test]
    public async Task GetTeam_ShouldReturnTheSpecifiedTeam()
    {
        var teamId = Random.Shared.Next();
        var team = new TeamEntity();

        _teamRepositoryMock.Setup(x => x.GetCompleteByIdAsync(teamId))
            .ReturnsAsync(team).Verifiable(Times.Once);

        // Act
        var actualTeam = await _service.GetTeamAsync(teamId);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamRepositoryMock);
    }
}