using Bingo.Api.TestUtils;

namespace Bingo.Api.Core.Tests;

[SetUpFixture]
public class ProjectTestSteps
{
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await TestProjectStepsUtil.RunBeforeAnyTests(BingoProjects.Core);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        TestProjectStepsUtil.RunAfterAnyTests(BingoProjects.Core);
    }
}