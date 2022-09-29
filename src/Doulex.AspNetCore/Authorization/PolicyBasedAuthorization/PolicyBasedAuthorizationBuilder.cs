using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.Authorization.PolicyBasedAuthorization;

/// <summary>
/// 包装的基于策略的认证声明和配置构建对象
/// </summary>
public class PolicyBasedAuthorizationBuilder
{
    private readonly IServiceCollection _services;

    internal PolicyBasedAuthorizationBuilder(IServiceCollection services)
    {
        _services = services;
    }

    /// <summary>
    /// 添加认证处理对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public PolicyBasedAuthorizationBuilder AddAuthorizationHandler<T>() where T : class, IAuthorizationHandler
    {
        _services.AddTransient<IAuthorizationHandler, T>();

        return this;
    }
}