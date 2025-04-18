using System.Net;
using NUnit.Framework;
using Socolin.ANSITerminalColor;
using Socolin.TestUtils.JsonComparer.Color;
using Socolin.TestUtils.JsonComparer.NUnitExtensions;

namespace Bingo.Api.TestUtils;

public static class Expect
{
    private static readonly JsonComparerColorOptions JsonColorOptions = new()
    {
        ColorizeDiff = true,
        ColorizeJson = true,
        Theme = new JsonComparerColorTheme
        {
            DiffAddition = AnsiColor.Background(TerminalRgbColor.FromHex("21541A")),
            DiffDeletion = AnsiColor.Background(TerminalRgbColor.FromHex("542822"))
        }
    };

    public static async Task StatusCodeFromResponse(HttpStatusCode code, HttpResponseMessage response)
    {
        if (response.StatusCode != code)
        {
            var stringContent = await response.Content.ReadAsStringAsync();
            if (stringContent.Length > 0) stringContent = " with " + stringContent;
            Assert.Fail($"Code {code} was expected, but server returned {response.StatusCode}{stringContent}");
        }
    }

    public static void EquivalentJsonWithPrettyOutput(string actualJson, string expectedJson)
    {
        Assert.That(actualJson, IsJson.EquivalentTo(expectedJson)
            .WithColoredOutput()
            .WithColorOptions(JsonColorOptions)
        );
    }

    public static async Task ResponseContentToMatchStatusCode(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var expectedContent = HttpHelper.GetExpectedJsonResponse(response.StatusCode);
        EquivalentJsonWithPrettyOutput(responseContent, expectedContent);
    }
}