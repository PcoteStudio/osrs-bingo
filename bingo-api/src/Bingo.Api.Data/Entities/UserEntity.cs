using System.ComponentModel.DataAnnotations.Schema;
using Bingo.Api.Data.Entities.Events;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Data.Entities;

[Table("Users")]
public class UserEntity : IdentityUser
{
    [ForeignKey("UserId")] public List<EventEntity> EventsAdministrated { get; set; } = [];
}