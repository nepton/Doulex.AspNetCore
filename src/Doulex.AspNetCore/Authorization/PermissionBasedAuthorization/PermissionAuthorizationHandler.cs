using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// Permission based authorization handler.
/// </summary>
public class PermissionAuthorizationHandler<TPermissions> : AuthorizationHandler<PermissionAuthorizationRequirement<TPermissions>>
{
    private readonly ILoginUserAuthorizationService                        _loginUserAuthorizationService;
    private readonly ILogger<PermissionAuthorizationHandler<TPermissions>> _logger;

    /// <inheritdoc />
    public PermissionAuthorizationHandler(
        ILogger<PermissionAuthorizationHandler<TPermissions>> logger,
        ILoginUserAuthorizationService                        loginUserAuthorizationService)
    {
        _logger                        = logger;
        _loginUserAuthorizationService = loginUserAuthorizationService;
    }

    /// <summary>
    /// Makes a decision if authorization is allowed based on a specific requirement.
    /// </summary>
    /// <param name="context">The authorization context.</param>
    /// <param name="requirement">The requirement to evaluate.</param>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext                      context,
        PermissionAuthorizationRequirement<TPermissions> requirement)
    {
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            _logger.LogWarning("缺少登录用户");
            return;
        }

        // 判断用户是否有权限访问该资源 如果用户没有权限, 则返回认证失败 401
        await _loginUserAuthorizationService.EnsureUserHasAuthorizedAsync(requirement.Scope, $"{requirement.Permission}");

        // 返回授权登录
        context.Succeed(requirement);
    }
}
