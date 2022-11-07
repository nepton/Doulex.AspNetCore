using Doulex.AspNetCore.Authorization.PolicyBasedAuthorization;
using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.Authorization.PermissionBasedAuthorization;

/// <summary>
/// Extension methods for adding permission based authorization services to the DI container. 
/// </summary>
public static class PermissionBasedAuthorizationDependencyInjection
{
    /// <summary>
    /// Add permission based authorization services to the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TPermissionBasedAuthorizationService">The permission authorization service</typeparam>
    /// <typeparam name="TPermissionBasedRequirement">The permission authorization metadata</typeparam>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddPermissionBasedAuthorization<TPermissionBasedAuthorizationService, TPermissionBasedRequirement>(this IServiceCollection services)
        where TPermissionBasedAuthorizationService : class, IPermissionBasedAuthorizationService<TPermissionBasedRequirement>
        where TPermissionBasedRequirement : PermissionBasedRequirement
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        services.AddTransient<TPermissionBasedAuthorizationService>();

        services.AddPolicyBasedAuthorization()
            .AddAuthorizationHandler<PermissionBasedAuthorizationHandler<TPermissionBasedAuthorizationService, TPermissionBasedRequirement>>();
    }
}
