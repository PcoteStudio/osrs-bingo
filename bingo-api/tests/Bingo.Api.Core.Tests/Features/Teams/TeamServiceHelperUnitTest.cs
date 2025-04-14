using System.Security.Claims;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Core.Features.Users;
using Bingo.Api.TestUtils.TestDataGenerators;
using FluentAssertions;
using Moq;

namespace Bingo.Api.Core.Tests.Features.Teams;

[TestFixture]
[TestOf(typeof(TeamService))]
public class TeamServiceHelperUnitTest
{
    [SetUp]
    public void BeforeEach()
    {
        _teamRepositoryMock = new Mock<ITeamRepository>(MockBehavior.Strict);
        _userServiceMock = new Mock<IUserService>(MockBehavior.Strict);
        _teamServiceHelperMock = new Mock<TeamServiceHelper>(
            _teamRepositoryMock.Object,
            _userServiceMock.Object
        ) { CallBase = true };
        _teamServiceHelper = _teamServiceHelperMock.Object;
    }

    private TeamServiceHelper _teamServiceHelper;
    private Mock<TeamServiceHelper> _teamServiceHelperMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<IUserService> _userServiceMock;

    #region GetRequiredTeamAsync

    [Test]
    public async Task GetRequiredTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();

        _teamRepositoryMock.Setup(x => x.GetCompleteByIdAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);

        // Act
        var actualTeam = await _teamServiceHelper.GetRequiredCompleteTeamAsync(team.Id);

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
        _teamServiceHelperMock.Setup(x => x.GetRequiredCompleteTeamAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);

        // Act
        var actualUser = await _teamServiceHelper.EnsureIsTeamAdminAsync(principalMock.Object, team.Id);

        // Assert
        actualUser.Should().Be(user);
        Mock.VerifyAll(_userServiceMock, _teamServiceHelperMock);
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
        _teamServiceHelperMock.Setup(x => x.GetRequiredCompleteTeamAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once);
        principalMock.Setup(x => x.Identity)
            .Returns(new ClaimsIdentity()).Verifiable(Times.Once);

        // Act
        var act = async () => await _teamServiceHelper.EnsureIsTeamAdminAsync(principalMock.Object, team.Id);

        // Assert thrown error
        await act.Should().ThrowAsync<UserIsNotATeamAdminException>();

        Mock.VerifyAll(_userServiceMock, _teamServiceHelperMock);
    }

    #endregion
}