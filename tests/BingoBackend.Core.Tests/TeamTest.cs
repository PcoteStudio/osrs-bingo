using BingoBackend.Core.Features.Team;

namespace BingoBackend.Core.Tests;

[TestFixture]
[TestOf(typeof(Team))]
public class TeamTest
{
    [Test]
    public void Create_A_Team()
    {
        new Team();
    }
}