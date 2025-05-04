using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcFactory
{
    NpcEntity Create(NpcCreateArguments args);
}

public class NpcFactory : INpcFactory
{
    public NpcEntity Create(NpcCreateArguments args)
    {
        var npc = new NpcEntity
        {
            Name = args.Name,
            KillsPerHour = args.KillsPerHour
        };
        if (args.Image is not null && args.Image.Length > 0)
            npc.Image = ImageHelper.IFormFileToBase64(args.Image);
        return npc;
    }
}