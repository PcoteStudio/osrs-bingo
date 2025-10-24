using Bingo.Api.Web.Drops;
using JetBrains.Annotations;

namespace Bingo.Api.Web.Items;

[PublicAPI]
public class ItemResponse : ItemShortResponse
{
    public List<DropResponse> Drops { get; set; } = [];
}