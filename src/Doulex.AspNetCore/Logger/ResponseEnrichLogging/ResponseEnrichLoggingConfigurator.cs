using Microsoft.AspNetCore.Builder;

namespace Doulex.AspNetCore.Logger.ResponseEnrichLogging
{
    /// <summary>
    /// 用户异常处理中间件
    /// </summary>
    public static class ResponseEnrichLoggingConfigurator
    {
        /// <summary>
        /// 处理由于用户错误导致的异常
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        public static void UseResponseEnrichLogging(this IApplicationBuilder app, Action<ResponseEnrichLoggingOptions>? configureOptions = null)
        {
            var options = new ResponseEnrichLoggingOptions();
            configureOptions?.Invoke(options);

            app.UseMiddleware<ResponseEnrichLoggingMiddleware>(options);
        }
    }
}
