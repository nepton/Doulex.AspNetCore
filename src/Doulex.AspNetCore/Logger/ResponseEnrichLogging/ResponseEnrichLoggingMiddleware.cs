using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Serilog;
using Serilog.Context;

namespace Doulex.AspNetCore.Logger.ResponseEnrichLogging
{
    /// <summary>
    /// 用户回应记录中间件
    /// </summary>
    public class ResponseEnrichLoggingMiddleware
    {
        private readonly RequestDelegate              _next;
        private readonly ResponseEnrichLoggingOptions _options;

        public static readonly string[] ResponseHeaderExcludes =
        {
            "Cookie",
            "Host",
            "Connection",
        };

        public ResponseEnrichLoggingMiddleware(
            RequestDelegate              next,
            ResponseEnrichLoggingOptions options)
        {
            _next    = next;
            _options = options;
        }

        public HttpResponse EnableRewind(
            HttpResponse response,
            int          bufferThreshold = 30720,
            long?        bufferLimit     = null)
        {
            if (response == null)
                throw new ArgumentNullException(nameof(response));
            var body = response.Body;

            if (!body.CanSeek)
            {
                var bufferingReadStream = new FileBufferingReadStream(body, bufferThreshold);
                response.Body = bufferingReadStream;
                response.RegisterForDispose(bufferingReadStream);
            }

            return response;
        }

        public async Task Invoke(HttpContext context)
        {
            // 如果用户没有启用回应日志记录, pass through middleware
            if (!_options.LoggingRequired)
            {
                await _next.Invoke(context);
                return;
            }

            bool logging = false;

            await using var responseBody = new MemoryStream();

            // 狸猫换太子
            var originalBodyStream = context.Response.Body;
            context.Response.Body = responseBody;

            try
            {
                // 中间件执行
                await _next.Invoke(context);

                // 换回来, 写结果到原来的回应流
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

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
            catch
            {
                // 记录异常结果
                if (_options.LogWhenException)
                    logging = true;

                throw;
            }
            finally
            {
                if (logging)
                {
                    // 日志记录
                    await LogResponseAsync(context, responseBody);
                }
            }
        }

        /// <summary>
        /// 记录用户回应
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseStream"></param>
        /// <returns></returns>
        private async Task LogResponseAsync(HttpContext context, MemoryStream responseStream)
        {
            // 回应头
            var header = context.Response.Headers
                .Where(c => ResponseHeaderExcludes.Contains(c.Key) == false)
                .ToDictionary(c => c.Key, c => c.Value.FirstOrDefault());

            // 回应文本
            var responseText = await TryReadResponseStream(context, responseStream);

            using (LogContext.PushProperty("SourceContext", typeof(ResponseEnrichLoggingMiddleware).FullName))
            using (LogContext.PushProperty("ResponseHeader", header))
            using (LogContext.PushProperty("ResponseBody", responseText))
            {
                // 记录用户回应细节
                Log.Write(_options.Level,
                    "Enrich Response: ContentType is {ContentType}, Length = {ContentLength} ({ContentLengthActual})",
                    context.Response.ContentType,
                    context.Response.ContentLength,
                    responseText?.Length ?? 0);
            }
        }

        /// <summary>
        /// 尝试读取回应内容
        /// </summary>
        /// <param name="context"></param>
        /// <param name="responseStream"></param>
        /// <returns></returns>
        private static async Task<string> TryReadResponseStream(HttpContext context, Stream responseStream)
        {
            if (!Regex.IsMatch(context.Response.ContentType ?? "", @"json|xml|text"))
                return $"<{context.Response.ContentType} IS NOT SUPPORTED>";

            responseStream.Seek(0, SeekOrigin.Begin);
            return await new StreamReader(responseStream).ReadToEndAsync();
        }
    }
}
