using AutoMapper;
using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Arguments;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Generic.Exceptions;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bingo.Api.Web.Tests.Teams;

[TestFixture]
[TestOf(typeof(TeamController))]
public class TeamControllerUnitTest
{
    [SetUp]
    public void BeforeEach()
    {
        _teamServiceMock = new Mock<ITeamService>();
        _teamServiceHelperMock = new Mock<ITeamServiceHelper>();
        _mapperMock = new Mock<IMapper>();
        _teamController = new TeamController(
            _teamServiceMock.Object,
            _teamServiceHelperMock.Object,
            _mapperMock.Object
        );
    }

    private TeamController _teamController;
    private Mock<ITeamService> _teamServiceMock;
    private Mock<ITeamServiceHelper> _teamServiceHelperMock;
    private Mock<IMapper> _mapperMock;

    [Test]
    public async Task GetTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = TestDataGenerator.GenerateTeamResponse();

        _teamServiceHelperMock.Setup(x => x.GetRequiredCompleteAsync(team.Id))
            .ReturnsAsync(team).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<TeamResponse>(team))
            .Returns(teamResponse).Verifiable(Times.Once());

        // Act
        var result = await _teamController.GetTeamAsync(team.Id);

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<TeamResponse>();
        var actualTeamResponse = objResult.Value as TeamResponse;
        actualTeamResponse.Should().BeSameAs(teamResponse);

        Mock.VerifyAll(_teamServiceHelperMock, _mapperMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonTeamExceptionsAndExpectedStatusCode))]
    public async Task GetTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();

        _teamServiceHelperMock.Setup(x => x.GetRequiredCompleteAsync(teamId))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.GetTeamAsync(teamId);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceHelperMock);
    }

    [Test]
    public async Task UpdateTeamAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = TestDataGenerator.GenerateTeamResponse();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = new TeamUpdateArguments();

        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, team.Id))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.UpdateTeamAsync(team.Id, args))
            .ReturnsAsync(team).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<TeamResponse>(team))
            .Returns(teamResponse).Verifiable(Times.Once());

        // Act
        var result = await _teamController.UpdateTeamAsync(identityContainer, team.Id, args);

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<TeamResponse>();
        var actualTeamResponse = objResult.Value as TeamResponse;
        actualTeamResponse.Should().BeSameAs(teamResponse);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock, _mapperMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonAccessTeamExceptionsAndExpectedStatusCode))]
    public async Task UpdateTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = new TeamUpdateArguments();
        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, teamId))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.UpdateTeamAsync(teamId, args))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.UpdateTeamAsync(identityContainer, teamId, args);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock);
    }

    [Test]
    public async Task UpdateTeamPlayersAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = TestDataGenerator.GenerateTeamResponse();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(0, 5));

        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, team.Id))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.UpdateTeamPlayersAsync(team.Id, args))
            .ReturnsAsync(team).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<TeamResponse>(team))
            .Returns(teamResponse).Verifiable(Times.Once());

        // Act
        var result = await _teamController.UpdateTeamPlayersAsync(identityContainer, team.Id, args);

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<TeamResponse>();
        var actualTeamResponse = objResult.Value as TeamResponse;
        actualTeamResponse.Should().BeSameAs(teamResponse);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock, _mapperMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonAccessTeamExceptionsAndExpectedStatusCode))]
    public async Task UpdateTeamPlayersAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(3));
        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, teamId))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.UpdateTeamPlayersAsync(teamId, args))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.UpdateTeamPlayersAsync(identityContainer, teamId, args);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceHelperMock, _teamServiceMock);
    }

    [Test]
    public async Task AddTeamPlayersAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = TestDataGenerator.GenerateTeamResponse();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(0, 5));

        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, team.Id))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.AddTeamPlayersAsync(team.Id, args))
            .ReturnsAsync(team).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<TeamResponse>(team))
            .Returns(teamResponse).Verifiable(Times.Once());

        // Act
        var result = await _teamController.AddTeamPlayersAsync(identityContainer, team.Id, args);

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<TeamResponse>();
        var actualTeamResponse = objResult.Value as TeamResponse;
        actualTeamResponse.Should().BeSameAs(teamResponse);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock, _mapperMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonAccessTeamExceptionsAndExpectedStatusCode))]
    public async Task AddTeamPlayersAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(3));
        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, teamId))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.AddTeamPlayersAsync(teamId, args))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.AddTeamPlayersAsync(identityContainer, teamId, args);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock);
    }

    [Test]
    public async Task RemoveTeamPlayerAsync_ShouldReturnTheUpdatedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = TestDataGenerator.GenerateTeamResponse();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var args = TestDataGenerator.GeneratePlayerName();

        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, team.Id))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.RemoveTeamPlayerAsync(team.Id, args))
            .ReturnsAsync(team).Verifiable(Times.Once());
        _mapperMock.Setup(x => x.Map<TeamResponse>(team))
            .Returns(teamResponse).Verifiable(Times.Once());

        // Act
        var result = await _teamController.RemoveTeamPlayerAsync(identityContainer, team.Id, args);

        // Assert status code
        result.Result.Should().BeOfType<ObjectResult>();
        var objResult = (result.Result as ObjectResult)!;
        objResult.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Assert response content
        objResult.Value.Should().BeOfType<TeamResponse>();
        var actualTeamResponse = objResult.Value as TeamResponse;
        actualTeamResponse.Should().BeSameAs(teamResponse);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock, _mapperMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonAccessTeamExceptionsAndExpectedStatusCode))]
    public async Task RemoveTeamPlayerAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        var identityContainer = new IdentityContainer(new UserIdentity(new UserEntity()));
        var playerName = TestDataGenerator.GeneratePlayerName();
        _teamServiceHelperMock.Setup(x => x.EnsureIsTeamAdminAsync(identityContainer.Identity, teamId))
            .Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.RemoveTeamPlayerAsync(teamId, playerName))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.RemoveTeamPlayerAsync(identityContainer, teamId, playerName);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceMock, _teamServiceHelperMock);
    }

    private static IEnumerable<TestCaseData> GetCommonTeamExceptionsAndExpectedStatusCode()
    {
        yield return new TestCaseData(new TeamNotFoundException(Random.Shared.Next()), StatusCodes.Status404NotFound);
    }

    private static IEnumerable<TestCaseData> GetCommonAccessTeamExceptionsAndExpectedStatusCode()
    {
        foreach (var testCaseData in GetCommonTeamExceptionsAndExpectedStatusCode())
            yield return testCaseData;
        yield return new TestCaseData(
            new UserIsNotATeamAdminException(Random.Shared.Next(), TestDataGenerator.GenerateUserName()),
            StatusCodes.Status403Forbidden);
    }
}