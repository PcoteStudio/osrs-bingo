using AutoMapper;
using Bingo.Api.Core.Features.Teams;
using Bingo.Api.Core.Features.Teams.Exceptions;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Generic.Exceptions;
using Bingo.Api.Web.Teams;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        _loggerMock = new Mock<ILogger<TeamController>>();
        _teamController = new TeamController(_teamServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    private TeamController _teamController;
    private Mock<ITeamService> _teamServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<TeamController>> _loggerMock;

    [Test]
    public async Task GetTeamAsync_ShouldReturnTheSpecifiedTeam()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();
        var teamResponse = new TeamResponse();

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
    }

    [Test]
    [TestCaseSource(nameof(GetGetTeamAsyncExceptionsAndExpectedStatusCode))]
    public async Task GetTeamAsync_ShouldReturnExpectedHttpStatusCodeOnKnownErrors(Exception exception,
        int expectedStatusCode)
    {
        // Arrange
        var teamId = Random.Shared.Next();
        _teamServiceMock.Setup(x => x.GetRequiredTeamAsync(teamId))
            .ThrowsAsync(exception).Verifiable(Times.Once());

        // Act
        var act = async () => await _teamController.GetTeamAsync(teamId);

        // Assert status code
        (await act.Should().ThrowAsync<HttpException>()).Which.StatusCode.Should().Be(expectedStatusCode);
    }

    private static IEnumerable<TestCaseData> GetCommonTeamExceptionsAndExpectedStatusCode()
    {
        yield return new TestCaseData(new TeamNotFoundException(Random.Shared.Next()), StatusCodes.Status404NotFound);
    }

    private static IEnumerable<TestCaseData> GetCommonAuthorizedTeamExceptionsAndExpectedStatusCode()
    {
        foreach (var testCaseData in GetCommonTeamExceptionsAndExpectedStatusCode())
            yield return testCaseData;
        yield return new TestCaseData(new UserNotFoundException(TestDataGenerator.GenerateUserName()),
            StatusCodes.Status401Unauthorized);
        yield return new TestCaseData(new InvalidAccessTokenException(), StatusCodes.Status401Unauthorized);
        yield return new TestCaseData(
            new UserIsNotATeamAdminException(Random.Shared.Next(), TestDataGenerator.GenerateUserName()),
            StatusCodes.Status403Forbidden);
    }

    private static IEnumerable<TestCaseData> GetGetTeamAsyncExceptionsAndExpectedStatusCode()
    {
        foreach (var testCaseData in GetCommonTeamExceptionsAndExpectedStatusCode())
            yield return testCaseData;
        yield return new TestCaseData(new TeamNotFoundException(Random.Shared.Next()), StatusCodes.Status404NotFound);
    }
}