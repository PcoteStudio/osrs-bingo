using System.ComponentModel.DataAnnotations;

namespace BingoBackend.Web.Team;

public class CreateTeamRequest
{
    [Required] public string Name { get; set; }
}