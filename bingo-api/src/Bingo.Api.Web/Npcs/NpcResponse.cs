using Bingo.Api.Web.Drops;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Npcs;

[PublicAPI]
public class NpcResponse : NpcShortResponse
{
    public List<DropResponse> Drops { get; set; } = [];
}