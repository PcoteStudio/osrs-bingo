using BingoBackend.Core.Features.Players;
using BingoBackend.Data;
using BingoBackend.TestUtils;
using BingoBackend.TestUtils.TestDataSetup;
using FluentAssertions;

namespace BingoBackend.Core.Tests.Features.Players;

[TestFixture]
[TestOf(typeof(PlayerRepository))]
public class PlayerRepositoryIntegrationTest
{
    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext(BingoProjects.Core);
        _testDataSetup = new TestDataSetup(_dbContext);
        _playerRepository = new PlayerRepository(TestSetupUtil.GetDbContext(BingoProjects.Core));
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
    }

    private ApplicationDbContext _dbContext;
    private TestDataSetup _testDataSetup;
    private IPlayerRepository _playerRepository;

    [Test]
    public async Task GetByNameAsync_ShouldReturnThePlayer()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(3)
            .AddPlayer(out var player)
            .AddPlayers(2);

        // Act
        var actualPlayer = await _playerRepository.GetByNameAsync(player.Name);

        // Assert
        actualPlayer.Should().NotBeNull();
        actualPlayer.Should().BeEquivalentTo(player);
    }

    [Test]
    public async Task GetByNameAsync_ShouldReturnNullIfPlayerDoesNotExist()
    {
        // Act
        var actualPlayer = await _playerRepository.GetByNameAsync("Unknown");

        // Assert
        actualPlayer.Should().BeNull();
    }

    [Test]
    public async Task GetByNamesAsync_ShouldReturnThePlayers()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(1)
            .AddPlayers(3, out var players)
            .AddPlayers(2);
        var names = players.Select(p => p.Name).ToList();

        // Act
        var actualPlayers = await _playerRepository.GetByNamesAsync(names);

        // Assert
        actualPlayers.Should().NotBeNull();
        actualPlayers.Should().BeEquivalentTo(players);
    }

    [Test]
    public async Task GetByNamesAsync_ShouldReturnOnlyTheExistingPlayers()
    {
        // Arrange
        _testDataSetup
            .AddPlayers(1)
            .AddPlayers(3, out var players)
            .AddPlayers(2);
        var names = players.Select(p => p.Name).ToList();
        names[1] = "Unknown";
        players.RemoveAt(1);

        // Act
        var actualPlayers = await _playerRepository.GetByNamesAsync(names);

        // Assert
        actualPlayers.Should().NotBeNull();
        actualPlayers.Should().BeEquivalentTo(players);
    }

    [Test]
    public async Task GetByNamesAsync_ShouldReturnEmptyListIfPlayersDoNotExist()
    {
        // Arrange
        _testDataSetup.AddPlayers(5);
        var names = Enumerable.Range(0, 5).Select(i => "Unknown_" + i).ToList();

        // Act
        var actualPlayers = await _playerRepository.GetByNamesAsync(names);

        // Assert
        actualPlayers.Should().NotBeNull();
        actualPlayers.Should().BeEmpty();
    }
}