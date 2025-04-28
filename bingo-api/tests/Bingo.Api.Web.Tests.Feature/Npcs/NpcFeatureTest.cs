using Bingo.Api.Data;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Npcs;

public partial class NpcFeatureTest
{
    private Uri _baseUrl;
    private HttpClient _client;
    private ApplicationDbContext _dbContext;
    private IWebHost _host;
    private TestDataSetup _testDataSetup;

    [OneTimeSetUp]
    public void BeforeAll()
    {
        _host = TestSetupUtil.GetStartedWebHost();
        _baseUrl = TestSetupUtil.GetRequiredHostUri(_host);
        _testDataSetup = TestSetupUtil.GetTestDataSetup(BingoProjects.Web);
    }

    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext(BingoProjects.Web);
        _client = new HttpClient();
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
        _client.Dispose();
    }
}