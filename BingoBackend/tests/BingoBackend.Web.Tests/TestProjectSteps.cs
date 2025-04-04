using BingoBackend.TestUtils;

namespace BingoBackend.Web.Tests;

[SetUpFixture]
public class ProjectTestSteps
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        TestProjectStepsUtil.RunBeforeAnyTests(BingoProjects.Web);
    }

    [OneTimeTearDown]
    public void RunAfterAnyTests()
    {
        TestProjectStepsUtil.RunAfterAnyTests(BingoProjects.Core);
    }
}