using AutoMapper;
using Bingo.Api.TestUtils.TestDataGenerators;
using Bingo.Api.Web.Teams;

namespace Bingo.Api.Web.Tests.Teams;

[TestFixture]
[TestOf(typeof(TeamMappingProfile))]
public class PlayersMappingProfileUnitTest
{
    [OneTimeSetUp]
    public void BeforeAll()
    {
        _mapper = new MapperConfiguration(
            c => { c.AddMaps(typeof(TeamMappingProfile).Assembly); }).CreateMapper();
    }

    private IMapper _mapper;

    [Test]
    public void TeamEntity_ShouldBeProperlyMappedToATeamResponse()
    {
        // Arrange
        var team = TestDataGenerator.GenerateTeamEntity();

        // Act
        var teamResponse = _mapper.Map<TeamResponse>(team);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(teamResponse.Id, Is.EqualTo(team.Id));
            Assert.That(teamResponse.Name, Is.EqualTo(team.Name));
        });
    }
}