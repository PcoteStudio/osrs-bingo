using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Bingo.Api.Data.Entities;

[Serializable]
[Table("Items")]
[Index(nameof(Name), IsUnique = true)]
public class ItemEntity
{
    public int Id { get; set; }

    [MaxLength(255)] public string Name { get; set; } = string.Empty;

    [MaxLength(255)] public string Image { get; set; } = string.Empty;

    [ForeignKey("ItemId")] public List<DropInfoEntity> DropInfos { get; set; } = [];
}