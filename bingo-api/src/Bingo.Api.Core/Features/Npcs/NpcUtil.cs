using Bingo.Api.Core.Features.Npcs.Arguments;
using Bingo.Api.Data.Entities;

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
        npc.Image = args.Image;
        npc.KillsPerHours = args.KillsPerHours;
    }
}