using Bingo.Api.Web.Items;
using Bingo.Api.Web.Npcs;

namespace Bingo.Api.Web.DropInfos;

[Serializable]
public class DropInfoResponse : DropInfoShortResponse
{
    public NpcShortResponse Npc { get; set; }
    public ItemShortResponse Item { get; set; }
}