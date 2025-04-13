using System.Security.Claims;
using Bingo.Api.Core.Features.Players;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.Data;
using Bingo.Api.TestUtils.TestDataGenerators;
using FluentAssertions;
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
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _playerServiceMock = new Mock<IPlayerService>(MockBehavior.Strict);
        _dbContextMock = new Mock<ApplicationDbContext>(MockBehavior.Loose);
        _teamServiceMock = new Mock<TeamService>(
            _teamFactoryMock.Object,
            _teamRepositoryMock.Object,
            _teamUtilMock.Object,
            _userServiceMock.Object,
            _playerServiceMock.Object,
            _dbContextMock.Object
        ) { CallBase = true };
        _teamService = _teamServiceMock.Object;
    }

    private TeamService _teamService;
    private Mock<TeamService> _teamServiceMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<ITeamFactory> _teamFactoryMock;
    private Mock<ITeamUtil> _teamUtilMock;
    private Mock<IUserService> _userServiceMock;
    private Mock<IPlayerService> _playerServiceMock;
    private Mock<ApplicationDbContext> _dbContextMock;

    #region CreateTeam

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
        var actualTeam = await _teamService.CreateTeamAsync(eventId, teamArgs);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamFactoryMock, _teamRepositoryMock, _dbContextMock);
    }

    #endregion

    #region GetRequiredTeamAsync

    [Test]
    public async Task GetRequiredTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();

        _teamRepositoryMock.Setup(x => x.GetCompleteByIdAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);

        // Act
        var actualTeam = await _teamService.GetRequiredCompleteTeamAsync(team.Id);

        // Assert
        Assert.That(actualTeam, Is.EqualTo(team));
        Mock.VerifyAll(_teamRepositoryMock);
    }

    #endregion

    #region EnsureIsTeamAdminAsync

    [Test]
    public async Task EnsureIsTeamAdminAsync_ShouldNotThrowIfTheUserIsATeamAdmin()
    {
        // Arrange
        var principalMock = new Mock<ClaimsPrincipal>();
        var user = TestDataGenerator.GenerateUserEntity();
        var eventEntity = TestDataGenerator.GenerateEventEntity();
        var team = TestDataGenerator.GenerateTeamEntity();

        eventEntity.Administrators.Add(user);
        team.Event = eventEntity;

        _userServiceMock.Setup(x => x.GetRequiredMeAsync(principalMock.Object))
            .ReturnsAsync(user).Verifiable(Times.Once);
        _teamServiceMock.Setup(x => x.GetRequiredCompleteTeamAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);

        // Act
        var actualUser = await _teamService.EnsureIsTeamAdminAsync(principalMock.Object, team.Id);

        // Assert
        actualUser.Should().Be(user);
        Mock.VerifyAll(_userServiceMock, _teamServiceMock);
    }

    [Test]
    public async Task EnsureIsTeamAdminAsync_ShouldThrowIfTheUserIsANotTeamAdmin()
    {
        // Arrange
        var principalMock = new Mock<ClaimsPrincipal>();
        var user = TestDataGenerator.GenerateUserEntity();
        var eventEntity = TestDataGenerator.GenerateEventEntity();
        var team = TestDataGenerator.GenerateTeamEntity();

        team.Event = eventEntity;

        _userServiceMock.Setup(x => x.GetRequiredMeAsync(principalMock.Object))
            .ReturnsAsync(user).Verifiable(Times.Once);
        _teamServiceMock.Setup(x => x.GetRequiredCompleteTeamAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);
        principalMock.Setup(x => x.Identity)
            .Returns(new ClaimsIdentity()).Verifiable(Times.Once);

        // Act
        var act = async () => await _teamService.EnsureIsTeamAdminAsync(principalMock.Object, team.Id);

        // Assert thrown error
        await act.Should().ThrowAsync<UserIsNotATeamAdminException>();

        Mock.VerifyAll(_userServiceMock, _teamServiceMock);
    }

    #endregion
}