using Bingo.Api.Core.Features.Authentication;
using Bingo.Api.Core.Features.Authentication.Exception;
using Bingo.Api.Core.Features.Events.Exceptions;
using Bingo.Api.Core.Features.Users.Exceptions;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Events;

public interface IEventServiceHelper
{
    Task EnsureIsEventAdminAsync(IIdentity? identity, int eventId);
    Task<EventEntity> GetRequiredByIdAsync(int eventId);
}

public class EventServiceHelper(
    IEventRepository eventRepository
) : IEventServiceHelper
{
    public async Task EnsureIsEventAdminAsync(IIdentity? identity, int eventId)
    {
        switch (identity)
        {
            case null:
                throw new UserIsNotLoggedInException();
            case UserIdentity userIdentity:
            {
                var eventEntity = await GetRequiredByIdAsync(eventId);
                if (eventEntity.Administrators.Any(a => a.Id == userIdentity.UserId))
                    return;
                throw new UserIsNotAnEventAdminException(eventId, userIdentity.User.Username);
            }
            default:
                throw new AccessHasNotBeenDefinedException();
        }
    }

    public virtual async Task<EventEntity> GetRequiredByIdAsync(int eventId)
    {
        var eventEntity = await eventRepository.GetCompleteByIdAsync(eventId);
        if (eventEntity == null) throw new EventNotFoundException(eventId);
        return eventEntity;
    }
}