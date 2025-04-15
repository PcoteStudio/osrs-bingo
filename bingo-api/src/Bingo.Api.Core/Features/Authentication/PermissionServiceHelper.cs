using Bingo.Api.Core.Features.Authentication.Exception;
using Bingo.Api.Core.Features.Users.Exceptions;

namespace Bingo.Api.Core.Features.Authentication;

public interface IPermissionServiceHelper
{
    void EnsureHasPermissions(IIdentity? identity, List<string> permissions);
}

public class PermissionServiceHelper : IPermissionServiceHelper
{
    public void EnsureHasPermissions(IIdentity? identity, List<string> permissions)
    {
        switch (identity)
        {
            case null:
                throw new UserIsNotLoggedInException();
            case UserIdentity userIdentity:
                foreach (var permission in permissions)
                    if (!userIdentity.User.Permissions.Contains(permission))
                        throw new UserIsMissingPermissionException(userIdentity.User.Username, permission);
                break;
            default:
                throw new AccessHasNotBeenDefinedException();
        }
    }
}