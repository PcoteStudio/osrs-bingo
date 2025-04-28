namespace Bingo.Api.Web.Items;

[Serializable]
public class ItemShortResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}