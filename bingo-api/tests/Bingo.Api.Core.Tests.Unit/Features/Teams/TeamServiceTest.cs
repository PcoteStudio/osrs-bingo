using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Data;
using Bingo.Api.TestUtils.TestDataGenerators;
using Moq;

namespace Bingo.Api.Core.Tests.Unit.Features.Teams;

[TestFixture]
[TestOf(typeof(TeamService))]
public class TeamServiceUnitTest
{
    [SetUp]
    public void BeforeEach()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>(MockBehavior.Strict);
        _teamServiceHelperMock = new Mock<ITeamServiceHelper>(MockBehavior.Strict);
        _teamFactoryMock = new Mock<ITeamFactory>(MockBehavior.Strict);
        _teamUtilMock = new Mock<ITeamUtil>(MockBehavior.Strict);
        _playerServiceMock = new Mock<IPlayerService>(MockBehavior.Strict);
        _dbContextMock = new Mock<ApplicationDbContext>(MockBehavior.Loose);
        _teamServiceMock = new Mock<TeamService>(
            _teamFactoryMock.Object,
            _teamRepositoryMock.Object,
            _teamUtilMock.Object,
            _teamServiceHelperMock.Object,
            _playerServiceMock.Object,
            _dbContextMock.Object
        ) { CallBase = true };
        _teamService = _teamServiceMock.Object;
    }

    private TeamService _teamService;
    private Mock<TeamService> _teamServiceMock;
    private Mock<ITeamServiceHelper> _teamServiceHelperMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITeamFactory> _teamFactoryMock;
    private Mock<ITeamUtil> _teamUtilMock;
    private Mock<IPlayerService> _playerServiceMock;
    private Mock<ApplicationDbContext> _dbContextMock;

    #region CreateTeam

    [Test]
    public async Task CreateTeam_ShouldCreateATeamWithTheSpecifiedArgumentsAndReturnIt()
    {
        // Arrange
        var eventId = Random.Shared.Next();
        var args = TestDataGenerator.GenerateTeamCreateArguments();
        var team = TestDataGenerator.GenerateTeamEntity();

        _teamFactoryMock.Setup(x => x.Create(eventId, args))
            .Returns(team).Verifiable(Times.Once);
        _teamRepositoryMock.Setup(x => x.Add(team))
            .Verifiable(Times.Once);
        _dbContextMock.Setup(x => x.SaveChangesAsync(CancellationToken.None))
            .ReturnsAsync(1).Verifiable(Times.Once);

        // Act
        var actualTeam = await _teamService.CreateTeamAsync(eventId, args);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamFactoryMock, _teamRepositoryMock, _dbContextMock);
    }

    #endregion
}