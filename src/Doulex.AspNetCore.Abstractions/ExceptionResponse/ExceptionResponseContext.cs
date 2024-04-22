using Microsoft.AspNetCore.Http;

namespace Doulex.AspNetCore.ExceptionResponse;

/// <summary>
/// User exception handling context
/// </summary>
public class ExceptionResponseContext
{
    public ExceptionResponseContext(HttpContext context, Exception exception)
    {
        Context   = context;
        Exception = exception;
    }

    /// <summary>
    /// To request a context, set the Response instead of modifying the context
    /// </summary>
    public HttpContext Context { get; }

    /// <summary>
    /// Exception objects
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// What is returned (if this is not a user exception, do not set the exception return value)
    /// </summary>
    public IResponseContent? Response { get; set; }
}
