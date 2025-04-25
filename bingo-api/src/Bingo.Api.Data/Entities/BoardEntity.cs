namespace Bingo.Api.Data.Entities;

[Serializable]
public abstract class BoardEntity
{
    private EventEntity? _event;
    public int Id { get; set; }
    public int EventId { get; set; }

    public EventEntity Event
    {
        get => _event.ThrowIfNotLoaded();
        set => _event = value;
    }
}