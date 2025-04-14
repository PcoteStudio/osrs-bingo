using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerUtil
{
    void UpdatePlayer(PlayerEntity player, PlayerUpdateArguments args);
}

public class PlayerUtil : IPlayerUtil
{
    public void UpdatePlayer(PlayerEntity player, PlayerUpdateArguments args)
    {
        player.Name = args.Name;
    }
}