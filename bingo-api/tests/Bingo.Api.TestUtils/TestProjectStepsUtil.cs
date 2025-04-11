using NUnit.Framework;

namespace Bingo.Api.TestUtils;

public static class TestProjectStepsUtil
{
    public static async Task RunBeforeAnyTests(BingoProjects project)
    {
        await TestSetupUtil.RecreateDatabase(project);
        var testDataSetup = TestSetupUtil.GetTestDataSetup(project);
        testDataSetup
            .AddUser()
            .AddEvent()
            .AddTeams(Random.Shared.Next(5, 15));
    }

    [OneTimeTearDown]
    public static void RunAfterAnyTests(BingoProjects project)
    {
    }
}