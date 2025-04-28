using Bingo.Api.Core.Features.Statistics;
using Bingo.Api.Data.Entities;
using FluentAssertions;

namespace Bingo.Api.Core.Tests.Unit.Features.Statistics;

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
    public void GetDropEhc(double? kpc, double? dropRate, double? expectedEhc)
    {
        var npc = new NpcEntity { KillsPerHour = kpc };
        var dropInfo = new DropEntity { Npc = npc, DropRate = dropRate };
        var ehc = _statisticsUtil.GetDropEhc(dropInfo);
        ehc.Should().BeApproximately(expectedEhc, 0.000001);
    }

    [Test]
    [TestCaseSource(nameof(GetDropsEhcAndExpectedEhc))]
    public void GetItemEhc(List<double?> dropInfoEhcs, double? expectedEhc)
    {
        var item = new ItemEntity { Drops = [] };
        foreach (var diEhc in dropInfoEhcs)
            item.Drops.Add(new DropEntity { Ehc = diEhc });
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
        yield return new TestCaseData(null, 400d, null);
        yield return new TestCaseData(31d, null, null);
        yield return new TestCaseData(6.5, 400d, 42.61538461538461);
        yield return new TestCaseData(31d, 381d, 8.516129032258064);
    }

    private static IEnumerable<TestCaseData> GetDropsEhcAndExpectedEhc()
    {
        yield return new TestCaseData(new List<double?> { null }, null)
            .SetArgDisplayNames("Drops EHC: [null], Expected EHC: null");
        yield return new TestCaseData(new List<double?> { null, null }, null)
            .SetArgDisplayNames("Drops EHC: [null, null], Expected EHC: null");
        yield return new TestCaseData(new List<double?> { 2.4 }, 2.4)
            .SetArgDisplayNames("Drops EHC: [2.4, Expected EHC: 2.4");
        yield return new TestCaseData(new List<double?> { null, 12.6 }, 12.6)
            .SetArgDisplayNames("Drops EHC: [null, 12.6], Expected EHC: 12.6");
        yield return new TestCaseData(new List<double?> { 72.3, 43.1, 65.8 }, 43.1)
            .SetArgDisplayNames("Drops EHC: [72.3, 43.1, 65.8], Expected EHC: 43.1");
    }
}