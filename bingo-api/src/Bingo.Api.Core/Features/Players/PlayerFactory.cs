using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Players;

public interface IPlayerFactory
{
    PlayerEntity Create(string name);
}

public class PlayerFactory : IPlayerFactory
{
    public PlayerEntity Create(string name)
    {
        return new PlayerEntity { Name = name };
    }
}