using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Users")]
public class UserEntity
{
    private List<string>? _permissions;
    public int Id { get; set; }
    [MaxLength(30)] public string Username { get; set; } = string.Empty;
    [MaxLength(30)] public string UsernameNormalized { get; set; } = string.Empty;
    [MaxLength(255)] public string Email { get; set; } = string.Empty;
    [MaxLength(255)] public string EmailNormalized { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; }
    [MaxLength(255)] public string HashedPassword { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public List<string> Permissions
    {
        get => _permissions.ThrowIfNotLoaded();
        set => _permissions = value;
    }
}