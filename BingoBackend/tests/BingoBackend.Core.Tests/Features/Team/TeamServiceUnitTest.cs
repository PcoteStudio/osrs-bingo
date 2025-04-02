using AutoMapper;
using BingoBackend.Core.Features.Team;
using BingoBackend.Data;
using BingoBackend.Data.Team;
using Moq;

namespace BingoBackend.Core.Tests.Features.Team;

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
        _dbContext = new Mock<ApplicationDbContext>(MockBehavior.Strict);
        _service = new TeamService(_teamFactoryMock.Object, _teamRepositoryMock.Object, _mapperMock.Object,
            _dbContext.Object);
    }

    private TeamService _service;
    private Mock<IMapper> _mapperMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITeamFactory> _teamFactoryMock;
    private Mock<ApplicationDbContext> _dbContext;

    [Test]
    public void CreateTeam()
    {
        // Arrange
        var teamArguments = new TeamCreateArguments();
        var teamEntity = new TeamEntity();
        var team = new Core.Features.Team.Team();

        _teamFactoryMock.Setup(x => x.Create(teamArguments))
            .Returns(teamEntity).Verifiable(Times.Once);
        _teamRepositoryMock.Setup(x => x.Add(teamEntity))
            .Verifiable(Times.Once);
        _dbContext.Setup(x => x.SaveChanges())
            .Returns(1).Verifiable(Times.Once);
        _mapperMock.Setup(x => x.Map<Core.Features.Team.Team>(teamEntity))
            .Returns(team).Verifiable(Times.Once);

        // Act
        var actualTeam = _service.CreateTeam(teamArguments);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamFactoryMock, _teamRepositoryMock, _mapperMock, _dbContext);
    }

    [Test]
    public async Task ListTeams()
    {
        // Arrange
        const int entitiesCount = 3;
        var teamEntities = Enumerable.Range(0, entitiesCount).Select(_ => new TeamEntity()).ToList();
        var teams = teamEntities.Select(_ => new Core.Features.Team.Team()).ToList();

        _teamRepositoryMock.Setup(x => x.GetAll())
            .ReturnsAsync(teamEntities).Verifiable(Times.Once);
        for (var i = 0; i < entitiesCount; i++)
        {
            var index = i;
            _mapperMock.Setup(x => x.Map<Core.Features.Team.Team>(teamEntities[index]))
                .Returns(teams[index]).Verifiable(Times.Once);
        }

        // Act
        var actualTeams = await _service.ListTeams();

        // Assert
        Assert.That(actualTeams, Is.EquivalentTo(teams));
        Mock.VerifyAll(_teamRepositoryMock, _mapperMock);
    }
}