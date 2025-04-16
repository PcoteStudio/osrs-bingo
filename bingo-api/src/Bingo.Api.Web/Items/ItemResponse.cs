using Bingo.Api.Web.Drops;

namespace Bingo.Api.Web.Items;

[Serializable]
public class ItemResponse : ItemShortResponse
{
    public List<DropResponse> Drops { get; set; } = [];
}