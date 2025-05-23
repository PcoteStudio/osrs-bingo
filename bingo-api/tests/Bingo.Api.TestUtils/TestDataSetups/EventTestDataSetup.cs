using Bingo.Api.Data.Entities;
using Bingo.Api.TestUtils.TestDataGenerators;

namespace Bingo.Api.TestUtils.TestDataSetups;

public partial class TestDataSetup
{
    public TestDataSetup AddEvent(Action<EventEntity>? customizer = null)
    {
        return AddEvent(out _, customizer);
    }

    public TestDataSetup AddEvent(out EventEntity eventEntity, Action<EventEntity>? customizer = null)
    {
        eventEntity = GenerateEventEntity();
        return SaveEntity(eventEntity, customizer);
    }

    public TestDataSetup AddEvents(int count, Action<EventEntity>? customizer = null)
    {
        return AddEvents(count, out _, customizer);
    }

    public TestDataSetup AddEvents(int count, out List<EventEntity> events, Action<EventEntity>? customizer = null)
    {
        events = Enumerable.Range(0, count).Select(_ =>
        {
            AddEvent(out var eventEntity, customizer);
            return eventEntity;
        }).ToList();
        return this;
    }

    private EventEntity GenerateEventEntity()
    {
        return new EventEntity
        {
            Administrators = [GetRequiredLast<UserEntity>()],
            Name = TestDataGenerator.GenerateEventName()
        };
    }
}