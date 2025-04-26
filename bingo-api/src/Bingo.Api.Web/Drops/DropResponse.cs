using Bingo.Api.Web.Items;
using Bingo.Api.Web.Npcs;

namespace Bingo.Api.Web.Drops;

[Serializable]
public class DropResponse : DropShortResponse
{
    public required NpcShortResponse Npc { get; set; }
    public required ItemShortResponse Item { get; set; }
}