using Bingo.Api.TestUtils.TestDataSetups;
using NUnit.Framework;

namespace Bingo.Api.TestUtils;

public static class TestProjectStepsUtil
{
    public static async Task RunBeforeAnyTests(BingoProjects project)
    {
        await TestSetupUtil.RecreateDatabase(project);

        // Add a random amount of teams to simulate an existing DB
        var dbContext = TestSetupUtil.GetDbContext(project);
        var testDataSetup = new TestDataSetup(dbContext);
        testDataSetup.AddEvent();
        testDataSetup.AddTeams(Random.Shared.Next(5, 15));
    }

    [OneTimeTearDown]
    public static void RunAfterAnyTests(BingoProjects project)
    {
    }
}