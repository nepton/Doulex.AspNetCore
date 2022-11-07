using Doulex.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// Permission based authorization handler.
/// </summary>
public class PermissionBasedAuthorizationHandler<TPermissionBasedAuthorizationService, TPermissionBasedRequirement> : AuthorizationHandler<TPermissionBasedRequirement>
    where TPermissionBasedAuthorizationService : IPermissionBasedAuthorizationService<TPermissionBasedRequirement>
    where TPermissionBasedRequirement : PermissionBasedRequirement
{
    private readonly TPermissionBasedAuthorizationService                                                                            _loginUserAuthorizationService;
    private readonly ILogger<PermissionBasedAuthorizationHandler<TPermissionBasedAuthorizationService, TPermissionBasedRequirement>> _logger;
    private readonly ILoginTokenService                                                                                              _loginTokenService;

    /// <inheritdoc />
    public PermissionBasedAuthorizationHandler(
        TPermissionBasedAuthorizationService                                                                            loginUserAuthorizationService,
        ILogger<PermissionBasedAuthorizationHandler<TPermissionBasedAuthorizationService, TPermissionBasedRequirement>> logger,
        ILoginTokenService                                                                                              loginTokenService)
    {
        _logger                        = logger;
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
            _logger.LogWarning("User is not authenticated");
            return;
        }

        // 判断用户是否有权限访问该资源 如果用户没有权限, 则返回认证失败 401
        await _loginUserAuthorizationService.EnsureAuthorizedAsync(loginToken, requirement);

        // 返回授权登录
        context.Succeed(requirement);
    }
}
