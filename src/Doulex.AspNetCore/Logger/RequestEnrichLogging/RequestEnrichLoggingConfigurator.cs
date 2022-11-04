using Microsoft.AspNetCore.Builder;

namespace Doulex.AspNetCore.Logger.RequestEnrichLogging
{
    /// <summary>
    /// 用户异常处理中间件
    /// </summary>
    public static class RequestEnrichLoggingConfigurator
    {
        /// <summary>
        /// 处理由于用户错误导致的异常
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        public static void UseRequestEnrichLogging(this IApplicationBuilder app, Action<RequestEnrichLoggingOptions>? configureOptions = null)
        {
            var options = new RequestEnrichLoggingOptions();
            configureOptions?.Invoke(options);

            app.UseMiddleware<RequestEnrichLoggingMiddleware>(options);
        }
    }
}
