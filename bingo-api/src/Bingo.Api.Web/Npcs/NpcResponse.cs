using Bingo.Api.Web.Drops;

namespace Bingo.Api.Web.Npcs;

[Serializable]
public class NpcResponse : NpcShortResponse
{
    public List<DropResponse> Drops { get; set; } = [];
}