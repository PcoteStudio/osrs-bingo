using System.Text.RegularExpressions;
using Bingo.Api.Core.Features.Authentication.Exception;
using Bingo.Api.Core.Features.Users.Exceptions;

namespace Bingo.Api.Core.Features.Authentication;

public interface IPermissionServiceHelper
{
    void EnsureHasPermissions(IIdentity? identity, List<string> requiredPermissions);
}

public class PermissionServiceHelper : IPermissionServiceHelper
{
    public void EnsureHasPermissions(IIdentity? identity, List<string> requiredPermissions)
    {
        switch (identity)
        {
            case null:
                throw new UserIsNotLoggedInException();
            case UserIdentity userIdentity:
                var userPermissions = ConvertPermissionsToRegex(userIdentity.User.Permissions);
                foreach (var permission in requiredPermissions)
                    if (!HasRequiredPermission(permission, userPermissions))
                        throw new UserIsMissingPermissionException(userIdentity.User.Username, permission);
                break;
            default:
                throw new AccessHasNotBeenDefinedException();
        }
    }

    private List<Regex> ConvertPermissionsToRegex(ICollection<string> userPermissions)
    {
        return userPermissions.Select(permission =>
                "^" + Regex.Escape(permission).Replace("\\*", ".*") + "$")
            .Select(regexPattern => new Regex(regexPattern, RegexOptions.Compiled))
            .ToList();
    }

    private bool HasRequiredPermission(string requiredPermission, ICollection<Regex> userPermissions)
    {
        return userPermissions.Any(pattern => pattern.IsMatch(requiredPermission));
    }
}