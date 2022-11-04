using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.ExceptionResponse;

/// <summary>
/// 用户异常处理构建
/// </summary>
public class ExceptionResponseBuilder
{
    private readonly IServiceCollection _services;

    internal ExceptionResponseBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public ExceptionResponseBuilder SetHandler<T>() where T : class, IExceptionResponseHandler
    {
        _services.AddTransient<IExceptionResponseHandler, T>();
        return this;
    }
}