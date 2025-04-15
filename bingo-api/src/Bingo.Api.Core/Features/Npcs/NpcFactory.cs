using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcFactory
{
    NpcEntity Create(NpcCreateArguments args);
}

public class NpcFactory : INpcFactory
{
    public NpcEntity Create(NpcCreateArguments args)
    {
        return new NpcEntity
        {
            Name = args.Name,
            Image = args.Image,
            InGameId = args.InGameId,
            KillsPerHours = args.KillsPerHours
        };
    }
}