using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Doulex.AspNetCore.SerilogExtensions;

/// <summary>
/// 用户请求记录中间件
/// </summary>
public class RequestEnrichLoggingMiddleware
{
    private readonly RequestDelegate             _next;
    private readonly RequestEnrichLoggingOptions _options;

    public RequestEnrichLoggingMiddleware(
        RequestDelegate             next,
        RequestEnrichLoggingOptions options)
    {
        _next    = next;
        _options = options;
    }

    public async Task Invoke(HttpContext context)
    {
        // enable request stream
        if (_options.LoggingFields.HasFlag(HttpFields.RequestBody))
            context.Request.EnableBuffering();
        
        // enable response stream TODO use pool
        var             originalBodyStream = context.Response.Body;
        await using var responseBodyCache  = _options.LoggingFields.HasFlag(HttpFields.ResponseBody) ? (context.Response.Body = new MemoryStream()) as MemoryStream : null;

        try
        {
            // Invoke the next delegate/middleware in the pipeline
            await _next.Invoke(context);
        }
        finally
        {
            if (responseBodyCache != null)
            {
                // 换回来, 写结果到原来的回应流
                responseBodyCache.Seek(0, SeekOrigin.Begin);
                await responseBodyCache.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }

            var dc = context.RequestServices.GetService<IDiagnosticContext>();
            if (dc != null)
            {
                if (_options.LoggingFields != HttpFields.None)
                    await LogRequestAsync(dc, context);

                if (_options.LoggingFields.HasFlag(HttpFields.Response))
                    await LogResponseAsync(dc, context, responseBodyCache);

                await LogOthersAsync(dc, context);
            }
        }
    }

    private Task LogOthersAsync(IDiagnosticContext dc, HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            dc.Set("EndpointName", endpoint.DisplayName);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// 记录请求内容
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="context"></param>
    private async Task LogRequestAsync(IDiagnosticContext dc, HttpContext context)
    {
        if (_options.LoggingFields.HasFlag(HttpFields.RequestProtocol))
            dc.Set("RequestProtocol", context.Request.Protocol);

        if (_options.LoggingFields.HasFlag(HttpFields.RequestMethod))
            dc.Set("RequestMethod", context.Request.Method);

        if (_options.LoggingFields.HasFlag(HttpFields.RequestScheme))
            dc.Set("RequestScheme", context.Request.Scheme);

        if (_options.LoggingFields.HasFlag(HttpFields.RequestPath))
            dc.Set("RequestPath", context.Request.Path);

        if (_options.LoggingFields.HasFlag(HttpFields.RequestQuery))
        {
            if (context.Request.QueryString.HasValue)
                dc.Set("RequestQuery", context.Request.QueryString, true);
        }

        if (_options.LoggingFields.HasFlag(HttpFields.RequestHeaders))
        {
            var headers = context.Request.Headers.Where(h => _options.IncludeRequestHeaders(h.Key));
            if (headers.Any())
                dc.Set("RequestHeaders", headers);
        }

        if (_options.LoggingFields.HasFlag(HttpFields.RequestUser))
        {
            if (context.User.Identity is { } identity)
            {
                dc.Set("RequestUser",
                    new
                    {
                        identity.Name,
                        identity.IsAuthenticated,
                        identity.AuthenticationType,
                    },
                    true);
            }
        }

        if (_options.LoggingFields.HasFlag(HttpFields.RequestBody))
        {
            var requestText = await TryReadRequestStream(context) ?? "";

            if (_options.RequestBodyMaxLength > 0 && _options.RequestBodyMaxLength < requestText.Length)
                requestText = requestText.Substring(0, _options.RequestBodyMaxLength);

            if (!string.IsNullOrEmpty(requestText))
                dc.Set("RequestBody", requestText);
        }
    }

    /// <summary>
    /// 记录用户请求
    /// </summary>
    /// <param name="dc"></param>
    /// <param name="context"></param>
    /// <param name="responseStream"></param>
    /// <returns></returns>
    private async Task LogResponseAsync(IDiagnosticContext dc, HttpContext context, MemoryStream? responseStream = null)
    {
        if (_options.LoggingFields.HasFlag(HttpFields.ResponseHeaders))
        {
            var headers = context.Response.Headers.Where(x => _options.IncludeResponseHeaders(x.Key));
            if (headers.Any())
                dc.Set("ResponseHeaders", headers);
        }

        if (_options.LoggingFields.HasFlag(HttpFields.ResponseBody))
        {
            if (responseStream != null)
            {
                var responseText = await TryReadResponseStream(context, responseStream) ?? "";

                if (_options.ResponseBodyMaxLength > 0 && _options.ResponseBodyMaxLength < responseText.Length)
                    responseText = responseText.Substring(0, _options.ResponseBodyMaxLength);

                if (!string.IsNullOrEmpty(responseText))
                    dc.Set("ResponseBody", responseText);
            }
        }

        return;
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

    /// <summary>
    /// 尝试读取回应内容
    /// </summary>
    /// <param name="context"></param>
    /// <param name="responseStream"></param>
    /// <returns></returns>
    private static async Task<string?> TryReadResponseStream(HttpContext context, Stream responseStream)
    {
        if (responseStream.Length <= 0)
            return null;

        if (context.Response.ContentType is not {Length: > 0} contentType)
            return $"<UNKNOWN CONTENT TYPE>";

        if (!Regex.IsMatch(contentType, @"json|xml|text"))
            return $"<{context.Response.ContentType} IS NOT SUPPORTED>";

        responseStream.Seek(0, SeekOrigin.Begin);
        return await new StreamReader(responseStream).ReadToEndAsync();
    }
}
