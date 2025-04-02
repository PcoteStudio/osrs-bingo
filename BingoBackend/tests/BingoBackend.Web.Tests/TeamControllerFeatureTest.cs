using System.Net;
using System.Net.Http.Headers;
using BingoBackend.TestUtils;
using Microsoft.AspNetCore.Hosting;

namespace BingoBackend.Web.Tests;

public class Tests
{
    private Uri _baseUrl;
    private IWebHost _host;

    [OneTimeSetUp]
    public void BeforeAll()
    {
        _host = TestSetup.BuildWebHost();
        _host.Start();
        _baseUrl = TestSetup.GetRequiredHostUri(_host);
    }

    [OneTimeTearDown]
    public void AfterAll()
    {
        _host.Dispose();
    }

    [SetUp]
    public void BeforeEach()
    {
        TestSetup.RecreateDatabase();
    }

    [Test]
    public async Task Create_A_Team_As_Admin()
    {
        var client = new HttpClient();
        var teamName = StringGenerator.GenerateRandomLength(10, 20);
        var jsonContent = /* language=json */
            $$"""{ "name": "{{teamName}}" }""";
        var stringContent = new StringContent(jsonContent, new MediaTypeHeaderValue("application/json"));
        var expectedContent = /* language=json */
            $$"""{ "id": { "__match": { "type": "integer" } }, "name": "{{teamName}}" }""";

        var response = await client.PostAsync(new Uri(_baseUrl, "/api/teams"), stringContent);

        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);
        var responseContent = await response.Content.ReadAsStringAsync();
        Expect.EquivalentJsonWithPrettyOutput(responseContent, expectedContent);

        var dbContext = TestSetup.GetApplicationDbContext();
        var persistedTeams = dbContext.Teams.ToList();
        Assert.That(persistedTeams, Has.Count.EqualTo(1));
        Assert.That(persistedTeams[0].Name, Is.EqualTo(teamName));
    }
}