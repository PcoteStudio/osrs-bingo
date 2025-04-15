using Bingo.Api.TestUtils;
using NUnit.Framework;

namespace BIngo.Api.Web.Tests.Feature;

[SetUpFixture]
public class WebFeatureTest
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