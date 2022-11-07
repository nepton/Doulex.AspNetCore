using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// 包装的基于策略的认证声明和配置 
/// </summary>
public static class AuthenticationServiceDependencyInjection
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services)
    {
        return services.AddScoped<ILoginTokenService, LoginTokenService>();
    }
}
