using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Bingo.Api.Data.Constants;

namespace Bingo.Api.Data.Entities.Events;

[Table("Grinds")]
public class GrindEntity
{
    public int Id { get; set; }

    public GrindTypes Type { get; set; }
    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    // Metric grind
    public decimal? Target { get; set; }
    [MaxLength(255)] public string Metric { get; set; } = string.Empty;

    // Drop grind
    public List<ItemEntity> Drops { get; set; } = [];
}