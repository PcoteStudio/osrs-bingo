using Bingo.Api.Web.Items;
using Bingo.Api.Web.Npcs;

namespace Bingo.Api.Web.Drops;

[Serializable]
public class DropResponse : DropShortResponse
{
    public NpcShortResponse Npc { get; set; }
    public ItemShortResponse Item { get; set; }
}