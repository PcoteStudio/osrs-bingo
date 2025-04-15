using Bingo.Api.TestUtils;
using NUnit.Framework;

namespace Bingo.Api.Core.Tests.Integration;

[SetUpFixture]
public class CoreIntegrationTest
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