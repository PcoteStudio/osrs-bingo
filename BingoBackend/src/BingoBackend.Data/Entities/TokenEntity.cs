using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BingoBackend.Data.Entities;

[Table("Tokens")]
public class TokenEntity
{
    [Key] public int Id { get; set; }

    [Required] [MaxLength(30)] public string Username { get; set; } = string.Empty;

    [Required] [MaxLength(200)] public string RefreshToken { get; set; } = string.Empty;

    [Required] public DateTime ExpiredAt { get; set; }
}