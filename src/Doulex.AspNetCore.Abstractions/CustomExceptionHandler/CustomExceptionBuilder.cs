using Microsoft.Extensions.DependencyInjection;

namespace Doulex.AspNetCore.CustomExceptionHandler;

/// <summary>
/// 用户异常处理构建
/// </summary>
public class CustomExceptionBuilder
{
    private readonly IServiceCollection _services;

    internal CustomExceptionBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public CustomExceptionBuilder SetExceptionHandler<T>() where T : class, ICustomExceptionHandler
    {
        _services.AddTransient<ICustomExceptionHandler, T>();
        return this;
    }
}