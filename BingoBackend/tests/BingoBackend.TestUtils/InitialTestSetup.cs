using NUnit.Framework;

namespace BingoBackend.TestUtils;

[SetUpFixture]
public class InitialTestSetup
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestSetupUtil.RecreateDatabase();

        // Add a random amount of teams to simulate an existing DB
        var dbContext = TestSetupUtil.GetDbContext();
        var testDataSetup = new TestDataSetup.TestDataSetup(dbContext);
        testDataSetup.AddTeams(Random.Shared.Next(5, 15));
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
    }
}