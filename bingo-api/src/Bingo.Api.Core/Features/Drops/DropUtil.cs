using Bingo.Api.Core.Features.Drops.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Drops;

public interface IDropUtil
{
    void UpdateDrop(DropEntity drop, DropUpdateArguments args);
    void UpdateDropEhc(DropEntity drop);
    int? GetAverageKillCount(double rate);
    double? GetDropEhc(DropEntity drop);
}

public class DropUtil : IDropUtil
{
    public void UpdateDrop(DropEntity drop, DropUpdateArguments args)
    {
        drop.ItemId = args.ItemId;
        drop.NpcId = args.NpcId;
        drop.DropRate = args.DropRate;
    }

    public void UpdateDropEhc(DropEntity drop)
    {
        drop.Ehc = GetDropEhc(drop);
    }

    public int? GetAverageKillCount(double rate)
    {
        if (rate <= 0) return null;

        var currentKc = 2;
        var inverseProbabilityPerKc = (rate - 1) / rate;
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
        if (drop.DropRate is null || drop.Npc.KillsPerHours is null) return null;
        var kc = GetAverageKillCount(drop.DropRate.Value);
        if (kc is null) return null;
        return kc / drop.Npc.KillsPerHours;
    }
}