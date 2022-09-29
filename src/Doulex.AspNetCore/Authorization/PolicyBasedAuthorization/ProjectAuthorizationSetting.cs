using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.Authorization.PolicyBasedAuthorization;

/// <summary>
/// 包装的基于策略的认证声明和配置 
/// </summary>
public static class PolicyBasedAuthorizationExtensions
{
    public static PolicyBasedAuthorizationBuilder AddPolicyBasedAuthorization(this IServiceCollection services)
    {
        // 注册基础处理模块
        services.AddSingleton<IAuthorizationPolicyProvider, PolicyBasedAuthorizePolicyProvider>();
        return new PolicyBasedAuthorizationBuilder(services);
    }
}