using Bingo.Api.TestUtils;

namespace Bingo.Api.Web.Tests;

[SetUpFixture]
public class ProjectTestSteps
{
    [OneTimeSetUp]
    public async Task RunBeforeAnyTests()
    {
        await TestProjectStepsUtil.RunBeforeAnyTests(BingoProjects.Web);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        TestProjectStepsUtil.RunAfterAnyTests(BingoProjects.Core);
    }
}