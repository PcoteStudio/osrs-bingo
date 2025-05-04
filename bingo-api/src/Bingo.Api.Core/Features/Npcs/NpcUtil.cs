using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Data.Entities;
using Bingo.Api.Shared;

namespace Bingo.Api.Core.Features.Npcs;

public interface INpcUtil
{
    void UpdateNpc(NpcEntity npc, NpcUpdateArguments args);
}

public class NpcUtil : INpcUtil
{
    public void UpdateNpc(NpcEntity npc, NpcUpdateArguments args)
    {
        npc.Name = args.Name;
        npc.KillsPerHour = args.KillsPerHour;

        if (args.Image is not null && args.Image.Length > 0)
            npc.Image = ImageHelper.IFormFileToBase64(args.Image);
        else
            npc.Image = null;
    }
}