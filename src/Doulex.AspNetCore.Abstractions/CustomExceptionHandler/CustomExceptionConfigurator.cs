using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.CustomExceptionHandler;

/// <summary>
/// 用户异常处理中间件
/// </summary>
public static class CustomExceptionConfigurator
{
    public static CustomExceptionBuilder AddCustomException(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        return new CustomExceptionBuilder(services);
    }

    /// <summary>
    /// 处理由于用户错误导致的异常
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configureOptions"></param>
    public static void UseCustomExceptionHandler(this IApplicationBuilder app, Action<CustomExceptionOptions>? configureOptions = null)
    {
        app.UseMiddleware<CustomExceptionMiddleware>();
    }
}
