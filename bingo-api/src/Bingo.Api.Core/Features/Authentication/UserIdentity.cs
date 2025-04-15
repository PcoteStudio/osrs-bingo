using Bingo.Api.Data.Entities;

namespace Bingo.Api.Core.Features.Authentication;

public interface IIdentity;

public class UserIdentity(UserEntity user) : IIdentity
{
    public UserEntity User { get; } = user;
    public int UserId { get; } = user.Id;
}

public record IdentityContainer(IIdentity? Identity);