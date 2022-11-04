using Doulex.AspNetCore.Authorization.PolicyBasedAuthorization;
using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// 项目的认证声明和配置 
/// </summary>
public static class PermissionBasedAuthorizationDependencyInjection
{
    /// <summary>
    /// 配置授权
    /// </summary>
    /// <param name="services"></param>
    public static void AddPermissionBasedAuthorization<TPermission>(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        // 授权接口
        services.AddPolicyBasedAuthorization()
            .AddAuthorizationHandler<PermissionAuthorizationHandler<TPermission>>();
    }
}
