using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Doulex.AspNetCore.ExceptionResponse;

/// <summary>
/// 用户异常处理程序, 这一类的异常信息不属于程序错误, 因此, 不在标准异常处理模块执行
/// </summary>
public class ExceptionResponseMiddleware
{
    private readonly RequestDelegate                      _next;
    private readonly ILogger<ExceptionResponseMiddleware> _logger;
    private readonly IExceptionResponseHandler?           _handler;

    public ExceptionResponseMiddleware(RequestDelegate next, ILogger<ExceptionResponseMiddleware> logger, IExceptionResponseHandler handler)
    {
        _next    = next;
        _logger  = logger;
        _handler = handler;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception ex)
        {
            if (!await HandleCustomExceptionAsync(httpContext, ex))
                throw;
        }
    }

    /// <summary>
    /// 处理用户异常
    /// </summary>
    /// <param name="context"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    private async Task<bool> HandleCustomExceptionAsync(HttpContext context, Exception exception)
    {
        // 如果用户没有定义接口, 报错并返回
        if (_handler == null)
        {
            _logger.LogWarning("No handle was found. Implement ICustomExceptionHandler and register it!");
            return false;
        }

        // 交由用户定义的模块处理
        var handleContext = new ExceptionResponseContext(context, exception);
        await _handler.HandleExceptionAsync(handleContext);

        // 如果用户没有设置回应, 返回 False, 
        if (handleContext.Response is not { } handleResponse)
            return false;

        // 如果认为这是一个用户异常, 设置返回
        var response = context.Response;
        response.StatusCode  = handleResponse.StatusCode;
        response.ContentType = handleResponse.ContentType;

        if (string.IsNullOrEmpty(handleResponse.Content) == false)
            await response.WriteAsync(handleContext.Response.Content);

        return true;
    }
}
