using NUnit.Framework;

namespace BingoBackend.TestUtils;

public static class TestProjectStepsUtil
{
    public static void RunBeforeAnyTests(BingoProjects project)
    {
        TestSetupUtil.RecreateDatabase(project);

        // Add a random amount of teams to simulate an existing DB
        var dbContext = TestSetupUtil.GetDbContext(project);
        var testDataSetup = new TestDataSetup.TestDataSetup(dbContext);
        // testDataSetup.AddTeams(Random.Shared.Next(5, 15));
    }

    [OneTimeTearDown]
    public static void RunAfterAnyTests(BingoProjects project)
    {
    }
}