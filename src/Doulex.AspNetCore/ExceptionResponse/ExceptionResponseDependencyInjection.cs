using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.ExceptionResponse;

/// <summary>
/// 用户异常处理中间件
/// </summary>
public static class ExceptionResponseDependencyInjection
{
    public static ExceptionResponseBuilder AddExceptionResponse(this IServiceCollection services)
    {
        if (services == null)
            throw new ArgumentNullException(nameof(services));

        return new ExceptionResponseBuilder(services);
    }

    /// <summary>
    /// 处理由于用户错误导致的异常
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configureOptions"></param>
    public static void UseExceptionResponseHandler(this IApplicationBuilder app, Action<ExceptionResponseOptions>? configureOptions = null)
    {
        app.UseMiddleware<ExceptionResponseMiddleware>();
    }
}
