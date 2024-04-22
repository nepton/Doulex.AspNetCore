using Doulex.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// Permission-based authorization handler.
/// </summary>
public class
    PermissionBasedAuthorizationHandler<TPermissionBasedAuthorizationService, TPermissionBasedRequirement> : AuthorizationHandler<TPermissionBasedRequirement>
    where TPermissionBasedAuthorizationService : IPermissionBasedAuthorizationService<TPermissionBasedRequirement>
    where TPermissionBasedRequirement : PermissionBasedRequirement
{
    private readonly TPermissionBasedAuthorizationService _loginUserAuthorizationService;
    private readonly ILoginTokenService                   _loginTokenService;

    /// <inheritdoc />
    public PermissionBasedAuthorizationHandler(
        TPermissionBasedAuthorizationService loginUserAuthorizationService,
        ILoginTokenService                   loginTokenService)
    {
        _loginTokenService             = loginTokenService;
        _loginUserAuthorizationService = loginUserAuthorizationService;
    }

    /// <summary>
    /// Makes a decision if authorization is allowed based on a specific requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TPermissionBasedRequirement requirement)
    {
        // if (context.User.Identity?.IsAuthenticated != true)
        var loginToken = await _loginTokenService.GetTokenAsync();

        if (loginToken == null)
        {
            // Do not log here, just return, system will tell the user (401)
            // _logger.LogWarning("User is not authenticated");
            return;
        }

        // Determines whether the user has permission to access the resource If the user does not have permissions, an authentication failure 401 is returned
        await _loginUserAuthorizationService.EnsureAuthorizedAsync(loginToken, requirement);

        // Go back to Authorize Login
        context.Succeed(requirement);
    }
}
