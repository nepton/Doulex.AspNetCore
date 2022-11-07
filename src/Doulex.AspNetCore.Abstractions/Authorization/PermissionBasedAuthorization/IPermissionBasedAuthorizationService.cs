using Doulex.AspNetCore.Authentication;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// Interface used to determine the permissions of login users
/// </summary>
public interface IPermissionBasedAuthorizationService<in TPermissionBasedRequirement>
    where TPermissionBasedRequirement : PermissionBasedRequirement
{
    /// <summary>
    /// Determine whether the current login user has the right to perform the operations with the specified permissions
    /// </summary>
    /// <param name="loginToken"></param>
    /// <param name="requirement"></param>
    /// <param name="cancel"></param>
    /// <returns></returns>
    Task EnsureAuthorizedAsync(UserToken loginToken, TPermissionBasedRequirement requirement, CancellationToken cancel = default);
}
