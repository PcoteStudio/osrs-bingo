using AutoMapper;
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
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _teamRepositoryMock = new Mock<ITeamRepository>(MockBehavior.Strict);
        _teamFactoryMock = new Mock<ITeamFactory>(MockBehavior.Strict);
        _teamUtilMock = new Mock<ITeamUtil>(MockBehavior.Strict);
        _playerServiceMock = new Mock<IPlayerService>(MockBehavior.Strict);
        _dbContext = new Mock<ApplicationDbContext>(MockBehavior.Strict);
        _service = new TeamService(
            _teamFactoryMock.Object,
            _teamRepositoryMock.Object,
            _teamUtilMock.Object,
            _playerServiceMock.Object,
            _mapperMock.Object,
            _dbContext.Object
        );
    }

    private TeamService _service;
    private Mock<IMapper> _mapperMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITeamFactory> _teamFactoryMock;
    private Mock<ITeamUtil> _teamUtilMock;
    private Mock<IPlayerService> _playerServiceMock;
    private Mock<ApplicationDbContext> _dbContext;

    [Test]
    public void CreateTeam_ShouldCreateATeamWithTheSpecifiedArgumentsAndReturnIt()
    {
        // Arrange
        var teamArguments = new TeamCreateArguments();
        var teamEntity = new TeamEntity();
        var team = new Team();

        _teamFactoryMock.Setup(x => x.Create(teamArguments))
            .Returns(teamEntity).Verifiable(Times.Once);
        _teamRepositoryMock.Setup(x => x.Add(teamEntity))
            .Verifiable(Times.Once);
        _dbContext.Setup(x => x.SaveChanges())
            .Returns(1).Verifiable(Times.Once);
        _mapperMock.Setup(x => x.Map<Team>(teamEntity))
            .Returns(team).Verifiable(Times.Once);

        // Act
        var actualTeam = _service.CreateTeam(teamArguments);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamFactoryMock, _teamRepositoryMock, _mapperMock, _dbContext);
    }

    [Test]
    public async Task ListTeams_ShouldReturnAllTeams()
    {
        // Arrange
        const int entitiesCount = 3;
        var teamEntities = Enumerable.Range(0, entitiesCount).Select(_ => new TeamEntity()).ToList();
        var teams = teamEntities.Select(_ => new Team()).ToList();

        _teamRepositoryMock.Setup(x => x.GetAllAsync())
            .ReturnsAsync(teamEntities).Verifiable(Times.Once);
        for (var i = 0; i < entitiesCount; i++)
        {
            var index = i;
            _mapperMock.Setup(x => x.Map<Team>(teamEntities[index]))
                .Returns(teams[index]).Verifiable(Times.Once);
        }

        // Act
        var actualTeams = await _service.ListTeamsAsync();

        // Assert
        Assert.That(actualTeams, Is.EquivalentTo(teams));
        Mock.VerifyAll(_teamRepositoryMock, _mapperMock);
    }

    [Test]
    public async Task GetTeam_ShouldReturnTheSpecifiedTeam()
    {
        var teamId = Random.Shared.Next();
        var teamEntity = new TeamEntity();
        var team = new Team();

        _teamRepositoryMock.Setup(x => x.GetCompleteByIdAsync(teamId))
            .ReturnsAsync(teamEntity).Verifiable(Times.Once);
        _mapperMock.Setup(x => x.Map<Team>(teamEntity))
            .Returns(team).Verifiable(Times.Once);

        // Act
        var actualTeam = await _service.GetTeamAsync(teamId);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamRepositoryMock, _mapperMock);
    }
}