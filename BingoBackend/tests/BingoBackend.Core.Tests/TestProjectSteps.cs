using BingoBackend.TestUtils;

namespace BingoBackend.Core.Tests;

[SetUpFixture]
public class ProjectTestSteps
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestProjectStepsUtil.RunBeforeAnyTests(BingoProjects.Core);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        TestProjectStepsUtil.RunAfterAnyTests(BingoProjects.Core);
    }
}