using Bingo.Api.Data;
using Bingo.Api.TestUtils;
using Bingo.Api.TestUtils.TestDataSetups;
using Microsoft.AspNetCore.Hosting;
using NUnit.Framework;

namespace Bingo.Api.Web.Tests.Feature.Authentication;

public partial class AuthenticationFeatureTest
{
    private Uri _baseUrl;
    private HttpClient _client;
    private ApplicationDbContext _dbContext;
    private IWebHost _host;
    private TestDataSetup _testDataSetup;

    [OneTimeSetUp]
    public async Task BeforeAll()
    {
        _host = TestSetupUtil.BuildWebHost();
        await _host.StartAsync();
        _baseUrl = TestSetupUtil.GetRequiredHostUri(_host);
    }

    [OneTimeTearDown]
    public void AfterAll()
    {
        _host.Dispose();
    }

    [SetUp]
    public void BeforeEach()
    {
        _dbContext = TestSetupUtil.GetDbContext(BingoProjects.Web);
        _testDataSetup = TestSetupUtil.GetTestDataSetup(BingoProjects.Web);
        _client = new HttpClient();
    }

    [TearDown]
    public void AfterEach()
    {
        _dbContext.Dispose();
        _client.Dispose();
    }
}