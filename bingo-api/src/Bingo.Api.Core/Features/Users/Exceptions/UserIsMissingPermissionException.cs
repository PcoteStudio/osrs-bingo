namespace Bingo.Api.Core.Features.Users.Exceptions;

public class UserIsMissingPermissionException(string username, string permission)
    : Exception($"User {username} is missing permission {permission}");