using Bingo.Api.Core.Features.DropInfos.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.DropInfos;

public interface IDropInfoUtil
{
    void UpdateDropInfo(DropInfoEntity dropInfo, DropInfoUpdateArguments args);
    void UpdateDropInfoEhc(DropInfoEntity dropInfo);
    int? GetAverageKillCount(double rate);
    double? GetDropEhc(DropInfoEntity dropInfo);
}

public class DropInfoUtil : IDropInfoUtil
{
    public void UpdateDropInfo(DropInfoEntity dropInfo, DropInfoUpdateArguments args)
    {
        dropInfo.ItemId = args.ItemId;
        dropInfo.NpcId = args.NpcId;
        dropInfo.DropRate = args.DropRate;
    }

    public void UpdateDropInfoEhc(DropInfoEntity dropInfo)
    {
        dropInfo.Ehc = GetDropEhc(dropInfo);
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

    public double? GetDropEhc(DropInfoEntity dropInfo)
    {
        if (dropInfo.DropRate is null || dropInfo.Npc.KillsPerHours is null) return null;
        var kc = GetAverageKillCount(dropInfo.DropRate.Value);
        if (kc is null) return null;
        return kc / dropInfo.Npc.KillsPerHours;
    }
}