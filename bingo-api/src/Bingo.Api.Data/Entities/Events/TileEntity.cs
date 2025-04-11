using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bingo.Api.Data.Entities.Events;

[Serializable]
[Table("Tiles")]
public class TileEntity
{
    [Key] public int Id { get; set; }
    public int Name { get; set; }
    public int Description { get; set; }
    public int GrindCountForCompletion { get; set; }
    public List<GrindProgressionEntity> GrindProgressions { get; set; } = [];
}