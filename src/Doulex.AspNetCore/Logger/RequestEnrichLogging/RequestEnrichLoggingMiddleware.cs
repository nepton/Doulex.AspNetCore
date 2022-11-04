using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Serilog.Context;

namespace Doulex.AspNetCore.Logger.RequestEnrichLogging
{
    /// <summary>
    /// 用户请求记录中间件
    /// </summary>
    public class RequestEnrichLoggingMiddleware
    {
        private readonly RequestDelegate             _next;
        private readonly RequestEnrichLoggingOptions _options;

        public static readonly string[] RequestHeaderExcludes =
        {
            "Cookie",
            "Host",
            "Connection",
        };

        public RequestEnrichLoggingMiddleware(
            RequestDelegate             next,
            RequestEnrichLoggingOptions options)
        {
            _next    = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            bool logging = false;

            // if we have no need to log, pass through
            if (!_options.LoggingRequired)
            {
                await _next.Invoke(context);
                return;
            }

            // enable request rewind
            context.Request.EnableBuffering();

            try
            {
                await _next.Invoke(context);

                // 记录成功结果
                if (context.Response.StatusCode < 400 && _options.LogWhenSuccess)
                    logging = true;

                // 记录客户端失败结果
                if (context.Response.StatusCode >= 400 && context.Response.StatusCode < 500 && _options.LogWhenClientError)
                    logging = true;

                // 记录服务器失败结果
                if (context.Response.StatusCode >= 500 && _options.LogWhenServerError)
                    logging = true;
            }
            catch (Exception)
            {
                // 记录异常结果
                if (_options.LogWhenException)
                    logging = true;

                throw;
            }
            finally
            {
                // 执行记录
                if (logging)
                    await LogRequestAsync(context);
            }
        }

        /// <summary>
        /// 记录用户请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task LogRequestAsync(HttpContext context)
        {
            // 请求头
            var header = context.Request.Headers
                .Where(c => RequestHeaderExcludes.Contains(c.Key) == false)
                .ToDictionary(c => c.Key, c => c.Value.FirstOrDefault());

            // 请求文本
            var requestText = await TryReadRequestStream(context);

            using (LogContext.PushProperty("SourceContext", typeof(RequestEnrichLoggingMiddleware).FullName))
            using (LogContext.PushProperty("RequestUrl", context.Request.GetDisplayUrl()))
            using (LogContext.PushProperty("RequestHeader", header))
            using (LogContext.PushProperty("RequestBody", requestText))
            using (LogContext.PushProperty("QueryString", context.Request.QueryString))
            {
                // 记录用户请求细节
                Log.Write(_options.Level,
                    "Enrich Request: ContentType is {ContentType}, Length = {ContentLength}",
                    context.Request.ContentType,
                    context.Request.ContentLength);
            }
        }

        /// <summary>
        /// 尝试读取请求内容
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static async Task<string?> TryReadRequestStream(HttpContext context)
        {
            var requestBody = context.Request.Body;
            if (requestBody.Length <= 0)
                return null;

            if (!requestBody.CanSeek || !requestBody.CanRead)
                return "<CALL Request.EnableBuffering()>";

            if (!Regex.IsMatch(context.Request.ContentType ?? "", @"json|xml|text"))
                return $"<{context.Request.ContentType} IS NOT SUPPORTED>";

            requestBody.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(requestBody);
            return await reader.ReadToEndAsync();
        }
    }
}
