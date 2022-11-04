# ExceptionResponse

ExceptionResponse is a middleware running at asp.net core.

It's used for convert exception to http response. It's very useful for api.

## Install

```bash
dotnet add package Doulex.AspNetCore
```

## Usage

```csharp
public class ExceptionResponseHandler : IExceptionResponseHandler
{
    public Task HandleAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = 500;
        return context.Response.WriteAsync(exception.Message);
    }
}


public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddExceptionResponse().SetHandler<ExceptionResponseHandler>();
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseExceptionResponse(); // <==== Add this line
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
}
```
