using Microsoft.AspNetCore.Http;

namespace Doulex.AspNetCore.CustomExceptionHandler;

/// <summary>
/// 用户异常处理上下文
/// </summary>
public class CustomExceptionContext
{
    public CustomExceptionContext(HttpContext context, Exception exception)
    {
        Context = context;
        Exception = exception;
    }

    /// <summary>
    /// 请求上下文, 请设置 Response 而不要修改上下文
    /// </summary>
    public HttpContext Context { get; }

    /// <summary>
    /// 异常对象
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// 返回的内容(如果这不是用户异常, 不要设置异常返回值)
    /// </summary>
    public IResponseContent? Response { get; set; }
}
