using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace Doulex.AspNetCore.SerilogExtensions
{
    /// <summary>
    /// 用户异常处理中间件
    /// </summary>
    public static class RequestEnrichLoggingConfigurator
    {
        /// <summary>
        /// 配置增强请求日志记录的参数
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEnrichSerilogRequestLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RequestEnrichLoggingOptions>(configuration);
            return services;
        }

        /// <summary>
        /// 处理由于用户错误导致的异常
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configureOptions"></param>
        public static void UseEnrichSerilogRequestLogging(this IApplicationBuilder app, Action<RequestEnrichLoggingOptions>? configureOptions = null)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestEnrichLoggingOptions>>()?.Value;
            options ??= new RequestEnrichLoggingOptions();
            configureOptions?.Invoke(options);

            app.UseSerilogRequestLogging();
            app.UseMiddleware<RequestEnrichLoggingMiddleware>(options);
        }
    }
}
