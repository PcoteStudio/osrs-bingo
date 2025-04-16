using Bingo.Api.Web.DropInfos;

namespace Bingo.Api.Web.Npcs;

[Serializable]
public class NpcResponse : NpcShortResponse
{
    public List<DropInfoResponse> DropInfos { get; set; } = [];
}