using BingoBackend.Core.Features.Teams;
using BingoBackend.Data;
using BingoBackend.TestUtils;
using BingoBackend.TestUtils.TestDataSetup;
using FluentAssertions;

namespace BingoBackend.Core.Tests.Features.Teams;

[TestFixture]
[TestOf(typeof(TeamRepository))]
public class TeamRepositoryIntegrationTest
{
    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext();
        _testDataSetup = new TestDataSetup(_dbContext);
        _teamRepository = new TeamRepository(TestSetupUtil.GetDbContext());
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
    }

    private ApplicationDbContext _dbContext;
    private TestDataSetup _testDataSetup;
    private ITeamRepository _teamRepository;

    [Test]
    public async Task GetCompleteByIdAsync_ShouldReturnTheFullTeam()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(3)
            .AddPlayers(4, out var players)
            .AddPlayers(2)
            .AddTeam(out var team, t => t.Players.AddRange(players));

        // Act
        var actualTeam = await _teamRepository.GetCompleteByIdAsync(team.Id);

        // Assert
        actualTeam.Should().NotBeNull();
        actualTeam.Players.Should().BeEquivalentTo(players, options => options.IgnoringCyclicReferences());
        actualTeam.Should().BeEquivalentTo(team, options => options.IgnoringCyclicReferences());
    }

    [Test]
    public async Task GetCompleteByIdAsync_ShouldReturnNullIfTeamDoesNotExist()
    {
        // Act
        var actualTeam = await _teamRepository.GetCompleteByIdAsync(1_000_000);

        // Assert
        actualTeam.Should().BeNull();
    }
}