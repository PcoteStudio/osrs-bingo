using System.Net;
using System.Net.Http.Headers;
using BingoBackend.TestUtils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;

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
        var teamName = StringGenerator.GenerateRandomLength(10, 20);
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

        await Expect.StatusCodeFromResponse(HttpStatusCode.Created, response);

        var contentText = await response.Content.ReadAsStringAsync();
        Expect.EquivalentJsonWithPrettyOutput(contentText, expectedContent);

        // var response2 = await client.GetAsync(new Uri(_baseUrl, "/api/teams"));
        //
        // await Expect.StatusCodeFromResponse(HttpStatusCode.OK, response2);
        // var contentText2 = await response.Content.ReadAsStringAsync();
        // Expect.EquivalentJsonWithPrettyOutput(contentText2, $"[{expectedContent}]");
    }
}