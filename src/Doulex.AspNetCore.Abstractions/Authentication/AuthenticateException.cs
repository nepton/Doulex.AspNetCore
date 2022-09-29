namespace Doulex.AspNetCore.Authentication;

/// <summary>
/// Token 未认证异常, 每一次请求携带的 Token 无法认证通过，抛出此异常
/// </summary>
public class AuthenticateException : Exception
{
    public AuthenticateException()
    {
    }

    public AuthenticateException(string message) : base(message)
    {
    }
}