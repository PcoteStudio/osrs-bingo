using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Socolin.ANSITerminalColor;
using Socolin.TestUtils.JsonComparer.Color;
using Socolin.TestUtils.JsonComparer.NUnitExtensions;

namespace BingoBackend.Web.Tests;

public class Tests
{
    private Uri _baseUrl;
    private IWebHost _host;

    [SetUp]
    public void BeforeEach()
    {
        _host = WebHost
            .CreateDefaultBuilder([])
            .UseStartup<Startup>()
            .UseUrls("http://127.0.0.1:0")
            .Build();
        _host.Start();
        _baseUrl = new Uri(
            _host.Services
                .GetRequiredService<IServer>()
                .Features.Get<IServerAddressesFeature>()!
                .Addresses.First()
        );
    }

    [TearDown]
    public void AfterEach()
    {
        _host.Dispose();
    }

    [Test]
    public async Task Create_A_Team_As_Admin()
    {
        var client = new HttpClient();
        var teamName = "Les Barboteux";
        var jsonContent = /* language=json */
            $$"""
              {
                "name": "{{teamName}}"
              }
              """;
        var stringContent = new StringContent(jsonContent)
        {
            Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
        };
        var expectedContent = /* language=json */
            $$"""
              {
                "id": {
                  "__match": {
                    "type": "integer"
                  }
                },
                "name": "{{teamName}}"
              }
              """;

        var response = await client.PostAsync(new Uri(_baseUrl, "/api/teams"), stringContent);

        var contentText = await response.Content.ReadAsStringAsync();
        if (response.StatusCode != HttpStatusCode.Created)
            Assert.Fail($"Server returned {response.StatusCode} with {contentText}");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var jsonColorOptions = new JsonComparerColorOptions
        {
            ColorizeDiff = true,
            ColorizeJson = true,
            Theme = new JsonComparerColorTheme
            {
                DiffAddition = AnsiColor.Background(TerminalRgbColor.FromHex("21541A")),
                DiffDeletion = AnsiColor.Background(TerminalRgbColor.FromHex("542822"))
            }
        };
        Assert.That(contentText,
            IsJson.EquivalentTo(expectedContent).WithColoredOutput().WithColorOptions(jsonColorOptions));
    }
}