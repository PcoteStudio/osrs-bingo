using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Bingo.Api.Data.Entities;

[Table("Users")]
public class UserEntity : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}