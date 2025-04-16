using Bingo.Api.Web.DropInfos;

namespace Bingo.Api.Web.Items;

[Serializable]
public class ItemResponse : ItemShortResponse
{
    public List<DropInfoResponse> DropInfos { get; set; } = [];
}