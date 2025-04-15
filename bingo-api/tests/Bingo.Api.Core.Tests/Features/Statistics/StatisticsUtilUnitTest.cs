using Bingo.Api.Core.Features.Statistics;
using Bingo.Api.Data.Entities;
using FluentAssertions;

namespace Bingo.Api.Core.Tests.Features.Statistics;

[TestFixture]
[TestOf(typeof(StatisticsUtil))]
public class StatisticsUtilUnitTest
{
    [OneTimeSetUp]
    public void BeforeAll()
    {
        _statisticsUtil = new StatisticsUtil();
    }

    private StatisticsUtil _statisticsUtil;

    [Test]
    [TestCaseSource(nameof(GetDropRatesAndExpectAverageKillCounts))]
    public void GetAverageKillCount(double? dropRate, int? expectedAverageKc)
    {
        var kc = _statisticsUtil.GetAverageKillCount(dropRate);
        kc.Should().Be(expectedAverageKc);
    }

    [Test]
    [TestCaseSource(nameof(GetKphDropRatesAndExpectedEhc))]
    public void GetItemEhc(List<Tuple<double?, double?>> kphAndDropRates, double? expectedEhc)
    {
        var item = new ItemEntity { DropInfos = [] };
        foreach (var kphAndDropRate in kphAndDropRates)
        {
            var npc = new NpcEntity { KillsPerHours = kphAndDropRate.Item1 };
            var dropInfo = new DropInfoEntity { Npc = npc, DropRate = kphAndDropRate.Item2 };
            item.DropInfos.Add(dropInfo);
        }

        var ehc = _statisticsUtil.GetItemEhc(item);
        ehc.Should().BeApproximately(expectedEhc, 0.000001);
    }

    private static IEnumerable<TestCaseData> GetDropRatesAndExpectAverageKillCounts()
    {
        yield return new TestCaseData(0d, null);
        yield return new TestCaseData(1d, 1);
        yield return new TestCaseData(1.5d, 1);
        yield return new TestCaseData(1.9d, 1);
        yield return new TestCaseData(2d, 1);
        yield return new TestCaseData(3d, 2);
        yield return new TestCaseData(4d, 2);
        yield return new TestCaseData(5d, 3);
        yield return new TestCaseData(6d, 4);
        yield return new TestCaseData(381d, 264);
        yield return new TestCaseData(400d, 277);
        yield return new TestCaseData(3000d, 2079);
        yield return new TestCaseData(5000d, 3465);
    }

    private static IEnumerable<TestCaseData> GetKphDropRatesAndExpectedEhc()
    {
        yield return new TestCaseData(new List<Tuple<double?, double?>>
                { new(null, 400d) }, null)
            .SetArgDisplayNames("(KPH: null, Drop rate: 400), Expected EHC: null");
        yield return new TestCaseData(new List<Tuple<double?, double?>>
                { new(31d, null) }, null)
            .SetArgDisplayNames("(KPH: 31, Drop rate: null), Expected EHC: null");
        yield return new TestCaseData(new List<Tuple<double?, double?>>
                { new(6.5, 400d) }, 42.61538461538461)
            .SetArgDisplayNames("(KPH: 6.5, Drop rate: 400), Expected EHC: 42.615384");
        yield return new TestCaseData(new List<Tuple<double?, double?>>
                { new(31d, 16256d), new(31d, 381d) }, 8.516129032258064)
            .SetArgDisplayNames("(KPH: 31, Drop rate: 16256), (KPH: 31, Drop rate: 381), Expected EHC: null");
    }
}