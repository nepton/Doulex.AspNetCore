namespace Doulex.AspNetCore.ExceptionResponse;

public class ExceptionResponseOptions
{
    public void SetHandler<T>() where T : IExceptionResponseHandler
    {
        HandlerType = typeof(T);
    }

    /// <summary>
    /// 异常处理类型
    /// </summary>
    public Type? HandlerType
    {
        get;
        private set;
    }
}