using System.ComponentModel.DataAnnotations.Schema;
using Bingo.Api.Data.Entities.Events;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Users")]
public class UserEntity : IdentityUser
{
    private List<EventEntity>? _eventsAdministrated;

    [ForeignKey("UserId")]
    public List<EventEntity> EventsAdministrated
    {
        get => _eventsAdministrated.ThrowIfNotLoaded();
        set => _eventsAdministrated = value;
    }
}