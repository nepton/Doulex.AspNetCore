namespace Doulex.AspNetCore.ExceptionResponse;

public interface IExceptionResponseHandler
{
    Task HandleExceptionAsync(ExceptionResponseContext context);
}