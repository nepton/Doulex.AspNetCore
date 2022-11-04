using Microsoft.AspNetCore.Authorization;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// Permission based authentication
/// </summary>
public class PermissionAuthorizationRequirement<TPermissions> : IAuthorizationRequirement
{
    public TPermissions Permission { get; }

    /// <summary>
    /// the scope of permission.
    /// for example: "consumer" for user or "admin" for system administrator
    /// </summary>
    public string Scope { get; }

    public PermissionAuthorizationRequirement(string scope, TPermissions permissionDemand)
    {
        Permission = permissionDemand;
        Scope      = scope;
    }
}
