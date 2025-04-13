using System.Security.Claims;
using AutoMapper;
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
        _mapperMock = new Mock<IMapper>();
        _teamController = new TeamController(_teamServiceMock.Object, _mapperMock.Object);
    }

    private TeamController _teamController;
    private Mock<ITeamService> _teamServiceMock;
    private Mock<IMapper> _mapperMock;

    [Test]
    public async Task GetTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = TestDataGenerator.GenerateTeamResponse();

        _teamServiceMock.Setup(x => x.GetRequiredTeamAsync(team.Id))
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

        Mock.VerifyAll(_teamServiceMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonTeamExceptionsAndExpectedStatusCode))]
    public async Task GetTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        _teamServiceMock.Setup(x => x.GetRequiredTeamAsync(teamId))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.GetTeamAsync(teamId);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonAccessTeamExceptionsAndExpectedStatusCode))]
    public async Task UpdateTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        var args = new TeamUpdateArguments();
        _teamServiceMock.Setup(x => x.UpdateTeamAsync(teamId, args))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.UpdateTeamAsync(teamId, args);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceMock);
    }

    [Test]
    [TestCaseSource(nameof(GetCommonAccessTeamExceptionsAndExpectedStatusCode))]
    public async Task UpdateTeamPlayersAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        var args = TestDataGenerator.GenerateTeamPlayersArguments(Random.Shared.Next(3));
        _teamServiceMock.Setup(x => x.EnsureIsTeamAdminAsync(It.IsAny<ClaimsPrincipal>(), teamId))
            .ReturnsAsync(new UserEntity()).Verifiable(Times.Once());
        _teamServiceMock.Setup(x => x.UpdateTeamPlayersAsync(teamId, args))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.UpdateTeamPlayersAsync(teamId, args);

        // Assert thrown error
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);

        Mock.VerifyAll(_teamServiceMock);
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