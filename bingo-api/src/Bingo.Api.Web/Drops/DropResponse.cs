using Bingo.Api.Web.Items;
using Bingo.Api.Web.Npcs;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Drops;

[PublicAPI]
public class DropResponse : DropShortResponse
{
    public required NpcShortResponse Npc { get; set; }
    public required ItemShortResponse Item { get; set; }
}