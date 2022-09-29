namespace Doulex.AspNetCore.CustomExceptionHandler;

public interface ICustomExceptionHandler
{
    Task HandleExceptionAsync(CustomExceptionContext context);
}