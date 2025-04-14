using Bingo.Api.Core.Features.Players.Arguments;
using Bingo.Api.Data.Entities.Events;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerFactory
{
    PlayerEntity Create(string name);
    PlayerEntity Create(PlayerCreateArguments args);
}

public class PlayerFactory : IPlayerFactory
{
    public PlayerEntity Create(string name)
    {
        return new PlayerEntity
        {
            Name = name,
            Teams = []
        };
    }

    public PlayerEntity Create(PlayerCreateArguments args)
    {
        return new PlayerEntity
        {
            Name = args.Name,
            Teams = []
        };
    }
}