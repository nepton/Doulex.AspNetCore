namespace Doulex.AspNetCore.CustomExceptionHandler;

public class CustomExceptionOptions
{
    public void SetHandler<T>() where T : ICustomExceptionHandler
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