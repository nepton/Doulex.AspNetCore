using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Doulex.AspNetCore.Authorization.PolicyBasedAuthorization;

/// <summary>
/// 系统的验证策略处理模块, 自定义该模块, 目的是支持从 PolicyName 生成 Requirement 从而直接让处理函数接手
/// </summary>
internal class PolicyBasedAuthorizePolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PolicyBasedAuthorizePolicyProvider(IOptionsSnapshot<AuthorizationOptions> options) : base(options)
    {
    }

    /// <summary>
    /// Gets a <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" /> from the given <paramref name="policyName" />
    /// </summary>
    /// <param name="policyName">The policy name to retrieve.</param>
    /// <returns>The named <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationPolicy" />.</returns>
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (string.IsNullOrEmpty(policyName))
        {
            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return await GetDefaultPolicyAsync();
        }

        var requirement = PolicyBasedAuthorizeAttribute.ParseRequirementFromPolicyName(policyName);

        var builder = new AuthorizationPolicyBuilder();
        builder.AddRequirements(requirement);

        return builder.Build();
    }
}
