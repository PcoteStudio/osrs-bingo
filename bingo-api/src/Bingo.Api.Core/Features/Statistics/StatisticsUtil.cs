using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Statistics;

public class StatisticsUtil
{
    public int? GetAverageKillCount(double? rate)
    {
        if (rate is null or <= 0) return null;

        var currentKc = 2;
        var inverseProbabilityPerKc = (double)((rate - 1) / rate);
        var lastProbability = inverseProbabilityPerKc;
        var currentProbability = lastProbability * inverseProbabilityPerKc;

        while (currentProbability >= 0.5)
        {
            lastProbability = currentProbability;
            currentProbability *= inverseProbabilityPerKc;
            currentKc++;
        }

        var diffLast = 0.5 - lastProbability;
        var diffCurrent = currentProbability - 0.5;
        return currentKc - (diffLast <= diffCurrent ? 0 : 1);
    }

    public double? GetDropEhc(DropEntity drop)
    {
        var kc = GetAverageKillCount(drop.DropRate);
        if (kc is null) return null;
        return kc / drop.Npc.KillsPerHour;
    }

    public double? GetItemEhc(ItemEntity item)
    {
        return item.Drops
            .Select(di => di.Ehc)
            .Where(ehc => ehc is not null)
            .Min() ?? null;
    }
}